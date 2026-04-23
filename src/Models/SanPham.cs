using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraSuaWeb.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public int MaSP { get; set; }

        public string? TenSP { get; set; }

        public int? MaDanhMuc { get; set; }

        public string? MoTa { get; set; }

        public string? HinhAnh { get; set; }

        public List<SizeSanPham> SizeSanPhams { get; set; } = new List<SizeSanPham>();
    }
}