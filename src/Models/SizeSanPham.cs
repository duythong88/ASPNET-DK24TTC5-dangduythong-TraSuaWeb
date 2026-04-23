using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraSuaWeb.Models
{
    [Table("SizeSanPham")]
    public class SizeSanPham
    {
        [Key]
        public int MaSize { get; set; }

        public string? TenSize { get; set; }

        public int MaSP { get; set; }

        public decimal DonGia { get; set; }

        [ForeignKey("MaSP")]
        public SanPham SanPham { get; set; }
    }
}