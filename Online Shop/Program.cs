using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Classes;
using Online_Shop.Repository.Intefaces;
using Online_Shop.Services.Classes;
using Online_Shop.Services.Interfaces;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// DataBase 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IProductCartRepository, ProductCartRepository>();
builder.Services.AddScoped<IProductCartService, ProductCartService>();

// For injection (user mnanager , ....)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;

    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

using var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await DbInitializer.SeedRolesAsync(roleManager);
await DbInitializer.SeedAdminAsync(userManager);
await DbInitializer.SeedSellersAsync(userManager);
DbInitializer.SeedCartStatus(dbContext);
DbInitializer.SeedCategories(dbContext);
await DbInitializer.SeedProductsAsync(dbContext, userManager);

app.Run();
