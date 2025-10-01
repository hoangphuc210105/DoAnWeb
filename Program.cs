using DoAnWeb.Models;
using DoAnWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connect VNPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();

// 1️ Đăng ký DbContext
builder.Services.AddDbContext<QLDTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLDTConnection"))
);

// 2️ Đăng ký session + IHttpContextAccessor
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// 3️ Add services MVC
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

// Bật Authentication / Authorization (nếu có)
app.UseAuthorization();

// 5️⃣ Route cho Areas (Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// 6️⃣ Route mặc định cho website
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Phone}/{action=Index}/{id?}"
);

app.Run();
