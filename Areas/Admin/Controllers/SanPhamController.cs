using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnWeb.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class SanPhamController : Controller
    {
        private readonly QLDTContext _context;

        public SanPhamController(QLDTContext context)
        {
            _context = context;
        }

        // GET: SanPham
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // số sản phẩm trên 1 trang
            int pageNumber = page ?? 1; // nếu null thì mặc định là trang 1

            var totalItems = await _context.Sanphams.CountAsync();

            var sanPhams = await _context.Sanphams
                                        .Include(s => s.MadongspNavigation)
                                        .OrderBy(s => s.Masp)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            // Truyền dữ liệu phân trang qua ViewBag
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(sanPhams);
        }


        // GET: SanPham/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.Sanphams
                                        .Include(s => s.MadongspNavigation)
                                        .FirstOrDefaultAsync(m => m.Masp == id);
            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // GET: SanPham/Create
        public IActionResult Create()
        {
            ViewData["Madongsp"] = new SelectList(_context.Dongsanphams, "Madongsp", "Tendong");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Madongsp,Tensp,Soluong,Gia,Giamgia,Mota,Ngaysanxuat,Color")] Sanpham sp, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // tạo thư mục wwwroot/images nếu chưa có
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    // tạo tên file duy nhất
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // lưu tên file vào DB
                    sp.Hinhanh = fileName;
                }

                _context.Add(sp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Madongsp"] = new SelectList(_context.Dongsanphams, "Madongsp", "Tendong", sp.Madongsp);
            return View(sp);
        }




        // GET: SanPham/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.Sanphams.FindAsync(id);
            if (sanPham == null) return NotFound();

            ViewData["Madongsp"] = new SelectList(_context.Dongsanphams, "Madongsp", "Tendong", sanPham.Madongsp);
            return View(sanPham);
        }

        // POST: SanPham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sanpham sanPham, IFormFile? file)
        {
            if (id != sanPham.Masp) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var sp = await _context.Sanphams.AsNoTracking().FirstOrDefaultAsync(x => x.Masp == id);
                    if (sp == null) return NotFound();

                    // Nếu có file upload mới
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        sanPham.Hinhanh = fileName; // lưu tên file
                    }
                    else
                    {
                        sanPham.Hinhanh = sp.Hinhanh; // giữ nguyên ảnh cũ
                    }

                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Masp)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Madongsp"] = new SelectList(_context.Dongsanphams, "Madongsp", "Tendong", sanPham.Madongsp);
            return View(sanPham);
        }


        // GET: SanPham/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.Sanphams
                                        .Include(s => s.MadongspNavigation)
                                        .FirstOrDefaultAsync(m => m.Masp == id);
            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // POST: SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.Sanphams.FindAsync(id);
            if (sanPham != null)
            {
                _context.Sanphams.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.Sanphams.Any(e => e.Masp == id);
        }
    }
}
