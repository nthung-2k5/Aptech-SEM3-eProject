using System.Globalization;
using System.Text;
using EntityFramework.Exceptions.SqlServer;
using FluentValidation;
using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services;
using GiveAID.Services.Abstractions;
using Hydro.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddFolderRouteModelConvention("/Components", model => model.Selectors.Clear());
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
});

builder.Services.AddHydro();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = "Cookies";
}).AddCookie("Cookies", options => { options.LoginPath = "/Login"; }).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    string secretKey = jwtSettings["Secret"] ?? "super_secret_default_key_replace_me_in_production_over_32_chars";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "GiveAID",
        ValidAudience = jwtSettings["Audience"] ?? "GiveAID",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwt_token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorizationBuilder()
        .AddPolicy("AdminOnly", policy => policy.RequireRole(nameof(UserRole.Admin)));

builder.Services.AddHttpContextAccessor();

var auditingInterceptor = new AuditingInterceptor();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString).AddInterceptors(auditingInterceptor).UseExceptionProcessor());

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddSingleton<IImageService, S3ImageService>();

builder.Services.AddScoped<IAboutUsSubpageService, AboutUsSubpageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDonationCauseService, DonationCauseService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IGalleryImageService, GalleryImageService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<INgoService, NgoService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IPaymentService, FakePaymentService>();
builder.Services.AddScoped<IProgrammeService, ProgrammeService>();
builder.Services.AddScoped<IUserInterestService, UserInterestService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // This applies all pending migrations

    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();
    await DbSeeder.SeedAsync(db, passwordService);
    
    var s3 = scope.ServiceProvider.GetRequiredService<IImageService>();
    await s3.EnsureBucketExists();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var enforcedCulture = new CultureInfo("en-US");

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enforcedCulture),
    SupportedCultures = [enforcedCulture],
    SupportedUICultures = [enforcedCulture]
};

localizationOptions.RequestCultureProviders.Clear();

app.UseHttpsRedirection();

app.UseRouting();
app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.UseStaticFiles();
app.MapRazorPages().WithStaticAssets();
app.UseHydro(builder.Environment);

app.Run();
