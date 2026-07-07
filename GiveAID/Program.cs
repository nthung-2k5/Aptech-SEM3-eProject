using EntityFramework.Exceptions.SqlServer;
using GiveAID.Data;
using GiveAID.Services;
using GiveAID.Services.Abstractions;
using Hydro.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddFolderRouteModelConvention("/Components", model => model.Selectors.Clear());
});

builder.Services.AddHydro();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
}).AddCookie("Cookies", options => { options.LoginPath = "/Login"; }).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    string secretKey = jwtSettings["Secret"] ?? "super_secret_default_key_replace_me_in_production_over_32_chars";

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "GiveAID",
        ValidAudience = jwtSettings["Audience"] ?? "GiveAID",
        IssuerSigningKey =
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
    };

    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwt_token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddHttpContextAccessor();

var auditingInterceptor = new AuditingInterceptor();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString).AddInterceptors(auditingInterceptor).UseExceptionProcessor());

builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IAboutUsSubpageService, AboutUsSubpageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDonationCauseService, DonationCauseService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IGalleryImageService, GalleryImageService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<INgoService, NgoService>();
builder.Services.AddScoped<IProgrammeService, ProgrammeService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // This applies all pending migrations
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.UseStaticFiles();
app.MapRazorPages().WithStaticAssets();
app.UseHydro(builder.Environment);

app.Run();
