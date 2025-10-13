using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnWeb.Filters;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class DonHangController : Controller
    {
        private readonly QLDTContext _context;

        public DonHangController(QLDTContext context)
        {
            _context = context;
        }

        // GET: Admin/DonHang
        // GET: Admin/DonHang
        public async Task<IActionResult> Index(int? status, DateTime? fromDate, DateTime? toDate)
        {
            ViewData["Title"] = "Danh sách đơn hàng";
            ViewData["PageType"] = "donhang";

            var query = _context.Donhangs
                                .Include(d => d.MakhNavigation)
                                .Include(d => d.MattNavigation)
                                .AsQueryable();

            if (status.HasValue && status.Value > 0)
            {
                query = query.Where(d => d.Matt == status.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(d => d.Ngaydat >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(d => d.Ngaydat <= toDate.Value);
            }

            var donHangs = await query.OrderByDescending(d => d.Ngaydat).ToListAsync();

            // Đẩy danh sách trạng thái ra ViewBag để render dropdown
            ViewBag.TrangThaiList = new SelectList(_context.Trangthaidonhangs, "Matt", "Tentt");

            return View(donHangs);
        }

        // GET: Admin/DonHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.Donhangs
                                        .Include(d => d.MakhNavigation)
                                        .Include(d => d.MattNavigation)
                                        .Include(d => d.Ctdonhangs)
                                        .ThenInclude(c => c.MaspNavigation)
                                        .FirstOrDefaultAsync(d => d.Madonhang == id);

            if (donHang == null) return NotFound();

            ViewData["Title"] = $"Chi tiết đơn hàng #{donHang.Madonhang}";
            ViewData["PageType"] = "donhang";

            return View(donHang);
        }


        // GET: Admin/DonHang/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.Donhangs
                                        .Include(d => d.MattNavigation)
                                        .FirstOrDefaultAsync(d => d.Madonhang == id);

            if (donHang == null) return NotFound();

            // Lấy danh sách trạng thái cho dropdown (bao gồm “Đã hủy”)
            ViewData["TrangThaiList"] = new SelectList(_context.Trangthaidonhangs, "Matt", "Tentt", donHang.Matt);

            ViewData["Title"] = $"Cập nhật trạng thái đơn hàng #{donHang.Madonhang}";
            ViewData["PageType"] = "donhang";

            return View(donHang);
        }


        // POST: Admin/DonHang/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, int Matt, bool Htgiaohang)
        {
            var donHang = await _context.Donhangs.FindAsync(id);
            if (donHang == null) return NotFound();

            donHang.Matt = Matt;
            donHang.Htgiaohang = Htgiaohang;

            // Nếu tick "Đã giao", tự động chuyển trạng thái sang 4
            if (Htgiaohang)
            {
                donHang.Matt = 4; // Đã giao
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // POST: Admin/DonHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.Donhangs.FindAsync(id);
            if (donHang != null)
            {
                _context.Donhangs.Remove(donHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
