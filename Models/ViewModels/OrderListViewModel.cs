using System;

namespace DoAnWeb.Models.ViewModels
{
    public class OrderListViewModel
    {
        public int Madonhang { get; set; }
        public DateTime Ngaydat { get; set; }
        public decimal TriGia { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}
