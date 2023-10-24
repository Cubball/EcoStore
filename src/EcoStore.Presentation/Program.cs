using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Project.Data;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddIdentity<AppUser, IdentityRole>(opts =>
    {
        opts.Password.RequiredLength = 8;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireDigit = false;
        opts.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(
        opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

await DbInitializer.Initialize(app.Services.CreateScope());

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();