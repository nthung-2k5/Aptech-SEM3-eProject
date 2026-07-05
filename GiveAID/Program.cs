using GiveAID.Data;
using GiveAID.Services.Abstractions;
using GiveAID.Services;
using Hydro.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHydro();

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
// builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAboutService, AboutService>();
// builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IMemberService, MemberService>();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseStaticFiles();
app.MapRazorPages().WithStaticAssets();
app.UseHydro(builder.Environment);

app.Run();
