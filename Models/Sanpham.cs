using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Sanpham
    {
        public Sanpham()
        {
            Ctdonhangs = new HashSet<Ctdonhang>();
            Giohangs = new HashSet<Giohang>();
        }

        public int Masp { get; set; }
        public int Madongsp { get; set; }
        public string Tensp { get; set; } = null!;
        public int Soluong { get; set; }
        public string? Hinhanh { get; set; }
        public decimal? Gia { get; set; }
        public decimal? Giamgia { get; set; }
        public string? Mota { get; set; }
        public DateTime? Ngaysanxuat { get; set; }
        public string? Color { get; set; }

        public virtual Dongsanpham MadongspNavigation { get; set; } = null!;
        public virtual ICollection<Ctdonhang> Ctdonhangs { get; set; }
        public virtual ICollection<Giohang> Giohangs { get; set; }
    }
}
