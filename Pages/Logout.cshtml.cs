using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoAnWeb.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xóa session
            HttpContext.Session.Clear();

            // Redirect tới page DangNhap
            return RedirectToPage("/DangNhap");
        }
    }
}
