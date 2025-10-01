using DoAnWeb.Models;
using DoAnWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class DonHangController : Controller
    {
        private readonly QLDTContext _context;

        public DonHangController(QLDTContext context)
        {
            _context = context;
        }

        // GET: /DonHang/Orders
        public IActionResult Orders()
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
                return RedirectToAction("DangNhap", "NguoiDung");

            var orders = _context.Donhangs
                .Include(d => d.MattNavigation)
                .Where(d => d.Makh == maKH.Value && d.MattNavigation.Tentt != "Đã hủy") // chỉ lấy đơn chưa hủy
                .OrderByDescending(d => d.Ngaydat)
                .Select(d => new OrderListViewModel
                {
                    Madonhang = d.Madonhang,
                    Ngaydat = d.Ngaydat,
                    TriGia = d.TriGia,
                    TrangThai = d.MattNavigation.Tentt
                })
                .ToList();

            ViewData["Title"] = "Danh sách đơn hàng";
            ViewData["PageType"] = "order";

            return View(orders);
        }

        // POST: /DonHang/Cancel/5
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
                return RedirectToAction("DangNhap", "NguoiDung");

            var order = _context.Donhangs
                .Include(d => d.MattNavigation)
                .FirstOrDefault(d => d.Madonhang == id && d.Makh == maKH.Value);

            if (order == null)
                return NotFound();

            if (order.MattNavigation.Tentt != "Chờ xác nhận")
                return BadRequest("Đơn hàng không thể hủy.");

            var canceledStatus = _context.Trangthaidonhangs.FirstOrDefault(t => t.Tentt == "Đã hủy");
            if (canceledStatus == null)
                return BadRequest("Không tìm thấy trạng thái 'Đã hủy'.");

            order.Matt = canceledStatus.Matt;
            _context.SaveChanges();

            return RedirectToAction("Orders"); // Khi redirect, Orders() sẽ không load đơn đã hủy
        }




        // GET: /DonHang/Details/5
        public IActionResult Details(int id)
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
                return RedirectToAction("DangNhap", "NguoiDung");

            var order = _context.Donhangs
                .Include(d => d.MattNavigation)
                .Include(d => d.Ctdonhangs)
                    .ThenInclude(c => c.MaspNavigation)
                .Where(d => d.Madonhang == id && d.Makh == maKH.Value)
                .Select(d => new OrderDetailsViewModel
                {
                    Madonhang = d.Madonhang,
                    Ngaydat = d.Ngaydat,
                    TriGia = d.TriGia,
                    TrangThai = d.MattNavigation.Tentt,
                    Items = d.Ctdonhangs.Select(c => new OrderItemViewModel
                    {
                        Masp = c.Masp,
                        TenSP = c.MaspNavigation.Tensp,
                        Soluong = c.Soluongsp,
                        Gia = c.Gia
                    }).ToList()
                })
                .FirstOrDefault();

            if (order == null)
                return NotFound();

            // 👉 Gán thông tin cho layout
            ViewData["Title"] = "Chi tiết đơn hàng";
            ViewData["PageType"] = "order";

            return View(order);
        }

    }
}
