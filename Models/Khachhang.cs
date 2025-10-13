using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Khachhang
    {
        public Khachhang()
        {
            Donhangs = new HashSet<Donhang>();
            Giohangs = new HashSet<Giohang>();
        }

        public int Makh { get; set; }
        public string? Tenkh { get; set; }
        public DateTime Ngaysinh { get; set; }
        public bool? Gioitinh { get; set; }
        public string? Diachi { get; set; }
        public string? Tendn { get; set; }
        public string? Email { get; set; }
        public string? Matkhau { get; set; }
        public string? Sdt { get; set; }
        public string? ThuHang { get; set; }  // Ví dụ: Bronze, Silver, Gold, Diamond


        public virtual ICollection<Donhang> Donhangs { get; set; }
        public virtual ICollection<Giohang> Giohangs { get; set; }
    }
}
