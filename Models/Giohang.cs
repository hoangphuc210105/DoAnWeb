using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Giohang
    {
        public int Magiohang { get; set; }
        public int Makh { get; set; }
        public int Masp { get; set; }
        public int Soluong { get; set; }

        public virtual Khachhang MakhNavigation { get; set; } = null!;
        public virtual Sanpham MaspNavigation { get; set; } = null!;
    }
}
