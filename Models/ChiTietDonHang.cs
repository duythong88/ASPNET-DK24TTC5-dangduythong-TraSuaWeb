using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraSuaWeb.Models
{
    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [Key]
        public int MaCTDH { get; set; }

        public int MaDonHang { get; set; }
        public int MaSP { get; set; }
        public int MaSize { get; set; }

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        [ForeignKey("MaDonHang")]
        public DonHang? DonHang { get; set; }

        [ForeignKey("MaSP")]
        public SanPham? SanPham { get; set; }

        [ForeignKey("MaSize")]
        public SizeSanPham? SizeSanPham { get; set; }
    }
}