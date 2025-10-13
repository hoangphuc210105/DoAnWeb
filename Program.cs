using DoAnWeb.Models;
using DoAnWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️ Đăng ký dịch vụ VNPay
builder.Services.AddScoped<IVnPayService, VnPayService>();

// 2️ Cấu hình DbContext (Kết nối SQL Server)
builder.Services.AddDbContext<QLDTContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLDTConnection"))
);

// 3️ Cấu hình Session và HttpContextAccessor
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// 4️ Thêm hỗ trợ Razor Pages + Controllers
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // ✅ Thêm dòng này để Razor Page hoạt động

var app = builder.Build();

// 5️ Cấu hình middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 6️ Kích hoạt Session
app.UseSession();

// 7️ Bật xác thực & phân quyền (nếu có)
app.UseAuthorization();

// 8️ Map route cho Areas (Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// 9️ Route mặc định cho Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Phone}/{action=Index}/{id?}"
);

// 10 Thêm cấu hình cho Razor Pages
app.MapRazorPages();

app.Run();
