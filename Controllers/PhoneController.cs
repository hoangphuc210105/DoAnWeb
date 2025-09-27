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


        // Trang danh sách iPhone (tách riêng)
        public async Task<IActionResult> SanPham(string? keyword)
        {
            ViewData["Title"] = "Danh sách iPhone";
            ViewData["PageType"] = "Phone";

            var products = from p in _context.Sanphams
                           join dsp in _context.Dongsanphams on p.Madongsp equals dsp.Madongsp
                           join loai in _context.Loaisps on dsp.Maloai equals loai.Maloai
                           where loai.Tenloai == "iPhone"
                           select p;

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Tensp.Contains(keyword));
                ViewData["SearchKeyword"] = keyword;
            }

            return View(await products.ToListAsync());
        }


        // Trang danh sách iPad
        public async Task<IActionResult> Ipad(string? keyword)
        {
            ViewData["Title"] = "Danh sách iPad";
            ViewData["PageType"] = "Ipad";

            var products = from p in _context.Sanphams
                           join dsp in _context.Dongsanphams on p.Madongsp equals dsp.Madongsp
                           join loai in _context.Loaisps on dsp.Maloai equals loai.Maloai
                           where loai.Tenloai == "iPad"
                           select p;

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Tensp.Contains(keyword));
                ViewData["SearchKeyword"] = keyword;
            }

            return View(await products.ToListAsync());
        }

        // Trang danh sách Phụ kiện
        public async Task<IActionResult> PhuKien(string? keyword)
        {
            ViewData["Title"] = "Danh sách Phụ kiện";
            ViewData["PageType"] = "Accessory";

            var products = from p in _context.Sanphams
                           join dsp in _context.Dongsanphams on p.Madongsp equals dsp.Madongsp
                           join loai in _context.Loaisps on dsp.Maloai equals loai.Maloai
                           where loai.Tenloai == "Phụ kiện"
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


        // POST: /Phone/AddToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            // Lấy MAKH từ session
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập trước khi thêm vào giỏ hàng.";
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            // Kiểm tra sản phẩm tồn tại
            var product = _context.Sanphams.FirstOrDefault(p => p.Masp == id);
            if (product == null)
            {
                return NotFound();
            }

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var cartItem = _context.Giohangs.FirstOrDefault(g => g.Makh == maKH.Value && g.Masp == id);
            if (cartItem != null)
            {
                // Nếu đã có, tăng số lượng
                cartItem.Soluong += quantity;
            }
            else
            {
                // Nếu chưa có, tạo mới
                cartItem = new Giohang
                {
                    Makh = maKH.Value,
                    Masp = id,
                    Soluong = quantity
                };
                _context.Giohangs.Add(cartItem);
            }

            _context.SaveChanges();

            // 👉 Cập nhật lại tổng số lượng giỏ hàng vào session
            var cartCount = _context.Giohangs
                                    .Where(g => g.Makh == maKH.Value)
                                    .Sum(g => g.Soluong);
            HttpContext.Session.SetInt32("CART_COUNT", cartCount);

            TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("SanPham"); // hoặc RedirectToAction("Details", new { id })
        }
    }
}



