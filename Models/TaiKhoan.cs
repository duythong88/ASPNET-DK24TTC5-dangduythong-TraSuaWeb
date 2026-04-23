using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraSuaWeb.Models
{
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        public int MaTK { get; set; }

        public string? TenDangNhap { get; set; }
        public string? MatKhau { get; set; }
        public string? LoaiTaiKhoan { get; set; }

        public int MaKH { get; set; }

        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }
    }
}