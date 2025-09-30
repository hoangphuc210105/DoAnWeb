namespace DoAnWeb.Models.VnPay
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }    // Mô tả đơn hàng
        public string TransactionId { get; set; }       // ID giao dịch bên VnPay
        public string OrderId { get; set; }             // ID đơn hàng bạn gửi
        public string PaymentMethod { get; set; }       // Phương thức thanh toán (ví dụ: VNPay QR, thẻ)
        public string PaymentId { get; set; }           // ID thanh toán, có thể trùng với TransactionId
        public bool Success { get; set; }               // Trạng thái thanh toán: true nếu thành công
        public string Token { get; set; }               // Token giao dịch (nếu VnPay trả)
        public string VnPayResponseCode { get; set; }   // Mã kết quả trả về từ VnPay (00 = thành công)
    }

}
