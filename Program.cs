using System.Runtime.InteropServices;
using GiaHuy.Models;
using GiaHuy.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<GiaHuyDbContext>(options=>{
    string connectionString = builder.Configuration.GetConnectionString("GiaHuy")??"";
    options.UseSqlServer(connectionString); 
});

builder.Services.AddIdentity<AppUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<GiaHuyDbContext>()
                .AddDefaultTokenProviders();
                
builder.Services.Configure<IdentityOptions> (options => {
    options.Password.RequireDigit = false; 
    options.Password.RequireLowercase = false; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequiredLength = 3; 
    options.Password.RequiredUniqueChars = 1; 

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); 
    options.Lockout.MaxFailedAccessAttempts = 5; 
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = 
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true; 
    
    options.SignIn.RequireConfirmedEmail = true;           
    options.SignIn.RequireConfirmedPhoneNumber = false; 
});
var mailSetting = builder.Configuration.GetSection("MailSetting");
builder.Services.Configure<MailSetting>(mailSetting);
builder.Services.AddTransient<IEmailSender,SendMailService>();

builder.Services.AddAuthentication()
                .AddGoogle(options=>{
                    IConfigurationSection  googleAuthenticationSection =  builder.Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthenticationSection["ClientID"];
                    options.ClientSecret = googleAuthenticationSection["ClientSecret"];
                    options.CallbackPath = "/login-google";
                });













var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
