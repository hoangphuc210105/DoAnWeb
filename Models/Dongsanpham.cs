using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Dongsanpham
    {
        public Dongsanpham()
        {
            Sanphams = new HashSet<Sanpham>();
        }

        public int Madongsp { get; set; }
        public string Tendong { get; set; } = null!;

        public virtual ICollection<Sanpham> Sanphams { get; set; }
    }
}
