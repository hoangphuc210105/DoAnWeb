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

        public IActionResult Profile()
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                return RedirectToPage("/DangNhap");
            }

            var user = _context.Khachhangs.FirstOrDefault(k => k.Makh == maKH.Value);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Thông tin cá nhân";
            ViewData["PageType"] = "auth";
            return View(user);
        }

        // GET: /NguoiDung/EditProfile
        public IActionResult EditProfile()
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                return RedirectToPage("/DangNhap");
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
                return RedirectToPage("/DangNhap");
            }

            if (ModelState.IsValid)
            {
                var user = _context.Khachhangs.FirstOrDefault(k => k.Makh == maKH.Value);
                if (user == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin
                user.Tenkh = model.Tenkh;
                user.Ngaysinh = model.Ngaysinh;
                user.Gioitinh = model.Gioitinh;
                user.Diachi = model.Diachi;
                user.Tendn = model.Tendn;
                user.Email = model.Email;
                user.Sdt = model.Sdt;
                // user.ThuHang không cho user chỉnh sửa trực tiếp (chỉ Admin hoặc hệ thống cập nhật)

                _context.SaveChanges();

                HttpContext.Session.Clear();
                return RedirectToPage("/DangNhap");
            }

            ViewData["Title"] = "Chỉnh sửa thông tin cá nhân";
            ViewData["PageType"] = "auth";
            return View(model);
        }
    }
}
