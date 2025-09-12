using System;
using System.Collections.Generic;

namespace DoAnWeb.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Madonhang { get; set; }
        public DateTime Ngaydat { get; set; }
        public decimal TriGia { get; set; }
        public string TrangThai { get; set; } = string.Empty;

        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public int Masp { get; set; }
        public string TenSP { get; set; } = string.Empty;
        public int Soluong { get; set; }
        public decimal Gia { get; set; }
    }
}
