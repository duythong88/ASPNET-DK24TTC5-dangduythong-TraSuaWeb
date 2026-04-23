using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraSuaWeb.Models
{
    [Table("DonHang")]
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        public int MaKH { get; set; }

        public DateTime NgayDat { get; set; }

        public decimal TongTien { get; set; }

        public string? TrangThai { get; set; }
        public string? ThanhToan { get; set; }

        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }


        public List<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public string? LoaiDonHang { get; set; }
    }
}