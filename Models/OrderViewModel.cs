namespace DoAnWeb.Models
{
    public class OrderViewModel
    {
        public int Madonhang { get; set; }
        public DateTime Ngaydat { get; set; }
        public decimal TriGia { get; set; }
        public string TrangThai { get; set; }
        public bool CanCancel => TrangThai == "Chờ xác nhận"; // chỉ cho hủy đơn khi đơn chưa xử lý

    }

}
