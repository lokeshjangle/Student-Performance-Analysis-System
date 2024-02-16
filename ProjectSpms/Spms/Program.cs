using Microsoft.EntityFrameworkCore;
using spms.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// builder.Services.AddDbContext<spms.Models.SiteDbContext>();
builder.Services.AddDbContext<SiteDbContext>(options =>
    options.UseMySQL("Data Source=192.168.255.49;Database=spms;User Id=root;Password=120801"));

builder.Services.AddAuthentication()
                .AddCookie(option => option.LoginPath = "");
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();
