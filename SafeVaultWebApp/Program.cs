using SafeVaultWebApp.Data;
using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Controllers;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure in-memory database for Identity
builder.Services.AddDbContext<SafeVaultDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options => { options.SignIn.RequireConfirmedAccount = true; options.Password.RequireDigit = false; options.Password.RequiredLength = 8; options.Password.RequireLowercase = false; options.Password.RequireNonAlphanumeric = false; options.Password.RequireUppercase = false; })
    .AddRoles<IdentityRole>() // Add roles support
    .AddEntityFrameworkStores<SafeVaultDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();