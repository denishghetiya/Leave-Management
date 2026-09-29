using Microsoft.EntityFrameworkCore;
using Leave.DBContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Leave.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EmailService>();

// Configure authentication using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "AuthCookie";
    });
//builder.Services.AddAuthentication("Cookies")
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/Account/Login";
//        options.LogoutPath = "/Account/Logout";
//        options.AccessDeniedPath = "/Account/AccessDenied";
//        options.Cookie.Name = "AuthCookie";
//    });


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Authentication & Authorization must be placed in this order
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Leave.DBContext;
//using Leave.Services;
//using YourNamespace.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<EmailService>();
//builder.Services.AddSingleton<JwtService>(); // Register JwtService

//// Configure Cookie Authentication
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login";
//        options.LogoutPath = "/Account/Logout";
//        options.AccessDeniedPath = "/Account/AccessDenied";
//        options.Cookie.Name = "AuthCookie";
//    });

//// Configure JWT Authentication
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings["Issuer"],
//        ValidAudience = jwtSettings["Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(key)
//    };
//});

//builder.Services.AddAuthorization();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();

//// Enable Authentication and Authorization
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");

//app.Run();






//using Microsoft.EntityFrameworkCore;
//using Leave.DBContext;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Leave.Services;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
////builder.Services.AddSession();
//builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<EmailService>();

//#region JWT
//// Configure authentication using cookies
//builder.Services.AddAuthentication("Cookies")
//    .AddCookie("Cookies",options =>
//    {
//        options.LoginPath = "/Account/Login";    // Redirect to login if not authenticated
//        options.LogoutPath = "/Account/Logout";  // Redirect after logout
//        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if unauthorized
//        options.Cookie.Name = "AuthCookie";
//    });

//builder.Services.AddAuthorization();
//#endregion

////var jwtSettings = builder.Configuration.GetSection("JwtSettings");
////var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

////builder.Services
////    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
////    .AddJwtBearer(options =>
////    {
////        options.TokenValidationParameters = new TokenValidationParameters
////        {
////            ValidateIssuer = true,
////            ValidateAudience = true,
////            ValidateLifetime = true,
////            ValidateIssuerSigningKey = true,
////            ValidIssuer = jwtSettings["Issuer"],
////            ValidAudience = jwtSettings["Audience"],
////            IssuerSigningKey = new SymmetricSecurityKey(key)
////        };
////    });

////builder.Services.AddAuthorization();
////builder.Services.AddControllersWithViews();
////builder.Services.AddScoped<JwtService>();

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
////app.UseSession();
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();

//#region JWT
//app.UseAuthentication();
//app.UseAuthorization();
//#endregion


//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}")
//    .WithStaticAssets();

//app.Run();


