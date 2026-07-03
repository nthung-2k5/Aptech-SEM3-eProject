using GiveAID;
using GiveAID.Services.Abstractions;
using GiveAID.Services;
using Hydro.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddFolderRouteModelConvention("/Components", model => model.Selectors.Clear());
});

builder.Services.AddHydro();

builder.Services.AddScoped<IAboutUsSubpageService, AboutUsSubpageService>();
builder.Services.AddScoped<IProgrammeService, ProgrammeService>();
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", options => { options.LoginPath = "/Login"; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(connectionString));

// builder.Services.AddScoped<IUserService, UserService>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.UseStaticFiles();
app.MapRazorPages().WithStaticAssets();
app.UseHydro(builder.Environment);

app.Run();
