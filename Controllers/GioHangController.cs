using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class GioHangController : Controller
    {
        private readonly QLDTContext _context;

        public GioHangController(QLDTContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            var cartItems = _context.Giohangs
                                    .Include(g => g.MaspNavigation)
                                    .Where(g => g.Makh == maKH.Value)
                                    .ToList();
            ViewBag.TotalPrice = cartItems.Sum(i => (i.MaspNavigation.Gia ?? 0) * i.Soluong);

            ViewData["Title"] = "Giỏ hàng của bạn";
            ViewData["PageType"] = "Phone";
            

            return View(cartItems);
        }

        // Xóa 1 sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int id)
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null) return RedirectToAction("DangNhap", "NguoiDung");

            var item = _context.Giohangs.FirstOrDefault(g => g.Masp == id && g.Makh == maKH.Value);
            if (item != null)
            {
                _context.Giohangs.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Cập nhật số lượng
        [HttpPost]
		public IActionResult UpdateCart(List<int> id, List<int> quantity)
		{
			int? maKH = HttpContext.Session.GetInt32("MAKH");
			if (maKH == null) return RedirectToAction("DangNhap", "NguoiDung");

			for (int i = 0; i < id.Count; i++)
			{
				var item = _context.Giohangs.FirstOrDefault(g => g.Masp == id[i] && g.Makh == maKH.Value);
				if (item != null && quantity[i] > 0)
				{
					item.Soluong = quantity[i];
				}
			}

			_context.SaveChanges();

			// Cập nhật lại số lượng trong session
			var cartCount = _context.Giohangs
									.Where(g => g.Makh == maKH.Value)
									.Sum(g => g.Soluong);
			HttpContext.Session.SetInt32("CART_COUNT", cartCount);
			return RedirectToAction("Index");
		}


        // GET: Hiển thị form nhập thông tin thanh toán
        [HttpGet]
        public IActionResult Checkout()
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null) return RedirectToAction("DangNhap", "NguoiDung");

            var cartItems = _context.Giohangs
                                    .Include(g => g.MaspNavigation)
                                    .Where(g => g.Makh == maKH.Value)
                                    .ToList();

            ViewBag.CartItems = cartItems;

            ViewData["Title"] = "Thanh toán";
            ViewData["PageType"] = "thanhtoan"; // để Layout phân biệt có/không banner

            return View(new CheckoutViewModel()); // form rỗng
        }

        // POST: Xử lý thanh toán
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null) return RedirectToAction("DangNhap", "NguoiDung");

            var cartItems = _context.Giohangs
                                    .Include(g => g.MaspNavigation)
                                    .Where(g => g.Makh == maKH.Value)
                                    .ToList();

            if (!ModelState.IsValid)
            {
                // Load lại giỏ hàng để view không bị trống
                ViewBag.CartItems = cartItems;
                return View(model);
            }

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // Tạo đơn hàng
            var donHang = new Donhang
            {
                Ngaydat = DateTime.Now,
                Makh = maKH.Value,
                Tennguoinhan = model.tenNguoiNhan,
                Diachinhan = model.diaChiNhan,
                Dienthoainhan = model.dienThoaiNhan,
                Matt = 1,
                Htgiaohang = false,
                Htthanhtoan = model.paymentMethod == "VNPAY",
                TriGia = cartItems.Sum(i => (i.MaspNavigation.Gia ?? 0) * i.Soluong)
            };

            _context.Donhangs.Add(donHang);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                _context.Ctdonhangs.Add(new Ctdonhang
                {
                    Madonhang = donHang.Madonhang,
                    Masp = item.Masp,
                    Gia = item.MaspNavigation.Gia ?? 0,
                    Soluongsp = item.Soluong
                });
            }

            _context.Giohangs.RemoveRange(cartItems);
            _context.SaveChanges();

            if (model.paymentMethod == "VNPAY")
            {
                var paymentInfo = new DoAnWeb.Models.VnPay.PaymentInformationModel
                {
                    Name = model.tenNguoiNhan,
                    OrderDescription = "Khách hàng Thanh toán giỏ hàng",
                    Amount = (double)donHang.TriGia,
                    OrderType = "billpayment"
                };

                var vnPayService = HttpContext.RequestServices.GetService(typeof(DoAnWeb.Services.IVnPayService)) as DoAnWeb.Services.IVnPayService;
                var url = vnPayService.CreatePaymentUrl(paymentInfo, HttpContext);
                return Redirect(url);
            }

            return RedirectToAction("OrderSuccess", new { orderId = donHang.Madonhang });


        }

        // Hiển thị thông báo đặt hàng thành công
        public IActionResult OrderSuccess(int orderId)
        {
            int? maKH = HttpContext.Session.GetInt32("MAKH");
            if (maKH == null) return RedirectToAction("DangNhap", "NguoiDung");

            var donHang = _context.Donhangs
                                  .Include(d => d.Ctdonhangs)
                                  .ThenInclude(ct => ct.MaspNavigation)
                                  .FirstOrDefault(d => d.Madonhang == orderId && d.Makh == maKH.Value);

            if (donHang == null)
            {
                return RedirectToAction("Index", "Phone");
            }

            ViewData["Title"] = "Đặt hàng thành công";
            ViewData["PageType"] = "Phone";

            return View(donHang);
        }

    }
}
