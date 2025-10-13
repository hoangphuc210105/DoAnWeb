using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DoAnWeb.Models;
using System.Linq;

namespace DoAnWeb.Pages
{
    public class DangNhapModel : PageModel
    {
        private readonly QLDTContext _context;

        public DangNhapModel(QLDTContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DangNhapInputModel Input { get; set; }

        public class DangNhapInputModel
        {
            public string Email { get; set; } = string.Empty;
            public string MatKhau { get; set; } = string.Empty;
            public bool GhiNho { get; set; }
        }

        public void OnGet()
        {
            ViewData["PageType"] = "auth";
        }

        public IActionResult OnPost()
        {
            ViewData["PageType"] = "auth";

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return Page();
            }

            var user = _context.Khachhangs.FirstOrDefault(u =>
                u.Email == Input.Email && u.Matkhau == Input.MatKhau);

            if (user != null)
            {
                HttpContext.Session.SetInt32("MAKH", user.Makh);
                HttpContext.Session.SetString("TENKH", user.Tenkh);

                // Điều hướng về trang chủ hoặc Phone
                return RedirectToPage("/Phone/Index");
            }

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return Page();
        }
    }
}
