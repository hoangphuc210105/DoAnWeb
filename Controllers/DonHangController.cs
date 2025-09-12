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
                .Where(d => d.Makh == maKH.Value)
                .OrderByDescending(d => d.Ngaydat)
                .Select(d => new OrderListViewModel
                {
                    Madonhang = d.Madonhang,
                    Ngaydat = d.Ngaydat,
                    TriGia = d.TriGia,
                    TrangThai = d.MattNavigation.Tentt
                })
                .ToList();

            // 👉 Gán thông tin cho layout
            ViewData["Title"] = "Danh sách đơn hàng";
            ViewData["PageType"] = "order";

            return View(orders);
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
