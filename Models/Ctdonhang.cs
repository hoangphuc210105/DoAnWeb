using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Ctdonhang
    {
        public int Madonhang { get; set; }
        public int Masp { get; set; }
        public decimal Gia { get; set; }
        public int Soluongsp { get; set; }

        public virtual Donhang MadonhangNavigation { get; set; } = null!;
        public virtual Sanpham MaspNavigation { get; set; } = null!;
    }
}
