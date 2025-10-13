using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnWeb.Models
{
    public partial class Donhang
    {
        public Donhang()
        {
            Ctdonhangs = new HashSet<Ctdonhang>();
        }

        public int Madonhang { get; set; }
        public DateTime Ngaydat { get; set; }
        public int Matt { get; set; }
        public int Makh { get; set; }
        public string? Tennguoinhan { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [RegularExpression(@"^[\p{L}\d\s,.-]+$", ErrorMessage = "Địa chỉ không hợp lệ")]
        public string? Diachinhan { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Điện thoại phải từ 10-11 số và chỉ chứa số")]
        public string? Dienthoainhan { get; set; }
        public bool? Htthanhtoan { get; set; }
        public bool? Htgiaohang { get; set; }
        public decimal TriGia { get; set; }



        public virtual Khachhang MakhNavigation { get; set; } = null!;
        public virtual Trangthaidonhang MattNavigation { get; set; } = null!;
        public virtual ICollection<Ctdonhang> Ctdonhangs { get; set; }
    }
}
