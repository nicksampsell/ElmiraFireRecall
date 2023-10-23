using ElmiraFireRecall;
using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;
using ElmiraFireRecall.Services;
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllUsers", policy => policy.RequireClaim("UserRole"));
    options.AddPolicy("Admin", policy => policy.RequireClaim("UserRole", UserRole.Administrator.ToString()));
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddTransient<IClaimsTransformation, CCClaimsTransformation>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<ElmiraFireRecall.Services.IMailService, ElmiraFireRecall.Services.MailService>();

builder.Services.AddRazorPages();

builder.Services.AddHostedService<LongRunningService>();
builder.Services.AddSingleton<BackgroundWorkerQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
