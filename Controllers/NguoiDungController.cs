using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models; // Chứa class KHACHHANG và DbContext
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace DoAnWeb.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly QLDTContext _context;

        public NguoiDungController(QLDTContext context)
        {
            _context = context;
        }

        // GET: /NguoiDung/DangKy
        public IActionResult DangKy()
        {
            ViewData["Title"] = "Đăng ký tài khoản";
            ViewData["PageType"] = "auth"; // bạn có thể dùng "auth" để layout không hiển thị banner home
            return View();
        }

        // POST: /NguoiDung/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKy(Khachhang model)
        {
            ViewData["Title"] = "Đăng ký tài khoản";
            ViewData["PageType"] = "auth";

            if (ModelState.IsValid)
            {
                var existingUser = _context.Khachhangs.FirstOrDefault(k => k.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email đã được đăng ký!");
                    return View(model);
                }

                _context.Khachhangs.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("DangNhap");
            }
            return View(model);
        }

        // GET: /NguoiDung/DangNhap
        public IActionResult DangNhap()
        {
            ViewData["Title"] = "Đăng nhập";
            ViewData["PageType"] = "auth";
            return View();
        }

        // POST: /NguoiDung/DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangNhap(string email, string matkhau)
        {
            ViewData["Title"] = "Đăng nhập";
            ViewData["PageType"] = "auth";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matkhau))
            {
                ModelState.AddModelError("", "Email và mật khẩu không được bỏ trống.");
                return View();
            }

            var user = _context.Khachhangs.FirstOrDefault(u => u.Email == email && u.Matkhau == matkhau);
            if (user != null)
            {
                HttpContext.Session.SetInt32("MAKH", user.Makh);
                HttpContext.Session.SetString("TENKH", user.Tenkh);

                return RedirectToAction("Index", "Phone");
            }
            else
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                return View();
            }
        }

        // GET: /NguoiDung/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa session
            return RedirectToAction("DangNhap");
        }

        // GET: /NguoiDung/Profile
        public IActionResult Profile()
        {
            // Lấy MAKH từ session
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                // Nếu chưa đăng nhập, chuyển về trang đăng nhập
                return RedirectToAction("DangNhap");
            }

            // Lấy thông tin khách hàng từ database
            var user = _context.Khachhangs.FirstOrDefault(k => k.Makh == maKH.Value);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Thông tin cá nhân";
            ViewData["PageType"] = "auth"; // ẩn banner home
            return View(user);
        }

        // GET: /NguoiDung/EditProfile
        public IActionResult EditProfile()
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                return RedirectToAction("DangNhap");
            }

            var user = _context.Khachhangs.FirstOrDefault(k => k.Makh == maKH.Value);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Chỉnh sửa thông tin cá nhân";
            ViewData["PageType"] = "auth";
            return View(user);
        }

        // POST: /NguoiDung/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(Khachhang model)
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                return RedirectToAction("DangNhap");
            }

            if (ModelState.IsValid)
            {
                var user = _context.Khachhangs.FirstOrDefault(k => k.Makh == maKH.Value);
                if (user == null)
                {
                    return NotFound();
                }

                // Cập nhật các thông tin cho phép sửa
                user.Tenkh = model.Tenkh;
                user.Ngaysinh = model.Ngaysinh;
                user.Gioitinh = model.Gioitinh;
                user.Diachi = model.Diachi;
                user.Tendn = model.Tendn;
                user.Email = model.Email;
                user.Sdt = model.Sdt;

                _context.SaveChanges();

                // Xóa session để buộc đăng nhập lại
                HttpContext.Session.Clear();

                // Chuyển về trang đăng nhập
                return RedirectToAction("DangNhap");
            }

            ViewData["Title"] = "Chỉnh sửa thông tin cá nhân";
            ViewData["PageType"] = "auth";
            return View(model);
        }  
    }
}
