using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraSuaWeb.Models;

namespace TraSuaWeb.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }

        public string? TenKH { get; set; }
        public string? Email { get; set; }
        public string? SoDT { get; set; }
        public string? DiaChi { get; set; }

        public List<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
        public List<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
