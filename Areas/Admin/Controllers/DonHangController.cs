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
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách đơn hàng";
            ViewData["PageType"] = "donhang";

            var donHangs = await _context.Donhangs
                                         .Include(d => d.MakhNavigation) // khách hàng
                                         .Include(d => d.MattNavigation) // trạng thái
                                         .OrderByDescending(d => d.Ngaydat)
                                         .ToListAsync();
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
