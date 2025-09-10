using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Nguoiquantri
    {
        public string UserAdmin { get; set; } = null!;
        public string PassAdmin { get; set; } = null!;
        public string? HoTen { get; set; }
        public string? VaiTro { get; set; }
    }
}
