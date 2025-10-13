using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DoAnWeb.Models;
using System.Linq;

namespace DoAnWeb.Pages
{
    public class DangKyModel : PageModel
    {
        private readonly QLDTContext _context;

        public DangKyModel(QLDTContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Khachhang Input { get; set; } = new();

        public void OnGet()
        {
            ViewData["PageType"] = "auth";
        }

        public IActionResult OnPost()
        {
            ViewData["PageType"] = "auth";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = _context.Khachhangs.FirstOrDefault(k => k.Email == Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email đã được đăng ký!");
                return Page();
            }

            _context.Khachhangs.Add(Input);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToPage("/NguoiDung/DangNhap");
        }
    }
}
