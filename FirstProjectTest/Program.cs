using FirstProjectTest.Data;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using FirstProjectTest.Repo.IRepository;
using FirstProjectTest.Repo.IServices;
using FirstProjectTest.Repo.Repository;
using FirstProjectTest.Repo.Services;
using FirstProjectTest.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IWalletRepository, WalletRepository>();
builder.Services.AddTransient<IDbConnection>(_ => new SqlConnection(connectionString));
builder.Services.AddTransient<IWalletService, WalletService>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddIdentity<User, IdentityRole>()
 //.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
       name: "wallet",
       pattern: "Wallet/{action=Index}/{id?}",
       defaults: new { controller = "Wallet" });

    endpoints.MapControllerRoute(
       name: "deposit",
       pattern: "Deposit/{action=Index}/{id?}",
       defaults: new { controller = "Deposit" });

    endpoints.MapControllerRoute(
        name: "withdraw",
        pattern: "Withdraw/{action=Index}/{id?}",
        defaults: new { controller = "Withdraw" });
    
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });
});
app.MapRazorPages();

app.Run();
