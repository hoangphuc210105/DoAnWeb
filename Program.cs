using DoAnWeb.Models;
using DoAnWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Connect VNPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();


// 1️⃣ Đăng ký DbContext
builder.Services.AddDbContext<QLDTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLDTConnection"))
);

// 2️⃣ Đăng ký session + IHttpContextAccessor
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// 3️⃣ Add services MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4️⃣ Bật Session trước Authorization
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Phone}/{action=Index}/{id?}");

app.Run();
