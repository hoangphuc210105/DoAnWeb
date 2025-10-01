using DoAnWeb.Filters;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize] // Chỉ admin mới truy cập
    public class HomeController : Controller
    {
        private readonly QLDTContext _context;

        public HomeController(QLDTContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Thống kê đơn hàng theo tháng
            var currentYear = DateTime.Now.Year;
            var labels = Enumerable.Range(1, 12).Select(m => $"Tháng {m}").ToArray();
            var data = labels.Select((label, index) =>
            {
                int month = index + 1;
                return _context.Donhangs.Count(d => d.Ngaydat.Year == currentYear && d.Ngaydat.Month == month);
            }).ToArray();

            ViewBag.Labels = labels;
            ViewBag.Data = data;

            return View();
        }
    }
}
