using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class PhoneController : Controller
    {
        private readonly QLDTContext _context;

        public PhoneController(QLDTContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm (có thể search theo keyword)
        public async Task<IActionResult> Index()
        {
            ViewData["PageType"] = "home";
            var products = await _context.Sanphams.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Search(string? keyword)
        {
            ViewData["PageType"] = "search";

            var products = from p in _context.Sanphams
                           select p;

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Tensp.Contains(keyword));
                ViewData["SearchKeyword"] = keyword; // nếu muốn hiển thị keyword trên view
            }

            return View("Index", await products.ToListAsync());
            // trả về view Index nhưng banner ẩn nhờ PageType
        }


        // Trang danh sách điện thoại (tách riêng)
        public async Task<IActionResult> SanPham(string? keyword)
        {
            ViewData["Title"] = "Danh sách điện thoại";
            ViewData["PageType"] = "Phone";

            var products = from p in _context.Sanphams
                           select p;

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Tensp.Contains(keyword));
                ViewData["SearchKeyword"] = keyword;
            }

            return View(await products.ToListAsync());
        }

        // Hiển thị chi tiết sản phẩm theo ID
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Sanphams.FirstOrDefaultAsync(p => p.Masp == id);
            if (product == null) return NotFound();

            ViewData["PageType"] = "details"; // không hiển thị banner
            return View(product);
        }
    }
}



