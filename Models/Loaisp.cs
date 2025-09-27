using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Loaisp
    {
        public Loaisp()
        {
            Dongsanphams = new HashSet<Dongsanpham>();
        }

        public int Maloai { get; set; }
        public string Tenloai { get; set; } = null!;

        public virtual ICollection<Dongsanpham> Dongsanphams { get; set; }
    }
}
