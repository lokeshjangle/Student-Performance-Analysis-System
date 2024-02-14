using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<spms.Models.SiteDbContext>();
builder.Services.AddAuthentication()
                .AddCookie(option => option.LoginPath = "");
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();
