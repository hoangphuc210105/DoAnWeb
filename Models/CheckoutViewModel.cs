using System.ComponentModel.DataAnnotations;

namespace DoAnWeb.Models
{
    public class CheckoutViewModel
    {
        public string tenNguoiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Điện thoại phải từ 10-11 số và chỉ chứa số")]
        public string dienThoaiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [RegularExpression(@"^[\p{L}\d\s,.-]+$", ErrorMessage = "Địa chỉ không hợp lệ")]
        public string diaChiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string paymentMethod { get; set; }
    }
}
