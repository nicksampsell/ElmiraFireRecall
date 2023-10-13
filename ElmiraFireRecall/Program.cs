using ElmiraFireRecall;
using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;
using ElmiraFireRecall.Settings;
using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FireDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllUsers", policy => policy.RequireClaim("UserRole"));
    options.AddPolicy("Supervisor", policy => policy.RequireClaim("UserRole", UserRole.Supervisor.ToString(), UserRole.Administrator.ToString(), UserRole.Developer.ToString()));
    options.AddPolicy("Administrator", policy => policy.RequireClaim("UserRole", UserRole.Administrator.ToString(), UserRole.Developer.ToString()));
    options.AddPolicy("Developer", policy => policy.RequireClaim("UserRole", UserRole.Developer.ToString()));
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddTransient<IClaimsTransformation, CCClaimsTransformation>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
