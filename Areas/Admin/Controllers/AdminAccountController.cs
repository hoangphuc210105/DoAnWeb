    using DoAnWeb.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace DoAnWeb.Areas.Admin.Controllers
    {
        [Area("Admin")]
        public class AdminAccountController : Controller
        {
            private readonly QLDTContext _context;

            public AdminAccountController(QLDTContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Login(string userAdmin, string passAdmin)
            {
                if (string.IsNullOrEmpty(userAdmin) || string.IsNullOrEmpty(passAdmin))
                {
                    ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                    return View();
                }

                var admin = _context.Nguoiquantris
                                    .FirstOrDefault(a => a.UserAdmin == userAdmin && a.PassAdmin == passAdmin);

                if (admin != null)
                {
                    HttpContext.Session.SetString("ADMIN_USER", admin.UserAdmin);
                    HttpContext.Session.SetString("ADMIN_ROLE", admin.VaiTro ?? "Admin");

                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu.";
                return View();
            }

            public IActionResult Logout()
            {
                HttpContext.Session.Remove("ADMIN_USER");
                HttpContext.Session.Remove("ADMIN_ROLE");
                return RedirectToAction("Login");
            }
        }
    }
