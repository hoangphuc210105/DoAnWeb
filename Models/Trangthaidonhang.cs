using System;
using System.Collections.Generic;

namespace DoAnWeb.Models
{
    public partial class Trangthaidonhang
    {
        public Trangthaidonhang()
        {
            Donhangs = new HashSet<Donhang>();
        }

        public int Matt { get; set; }
        public string Tentt { get; set; } = null!;

        public virtual ICollection<Donhang> Donhangs { get; set; }
    }
}
