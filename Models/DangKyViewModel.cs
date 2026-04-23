using System.ComponentModel.DataAnnotations;

namespace TraSuaWeb.Models
{
    public class DangKyViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = string.Empty;
        public string? TenKH { get; set; }
        public string? SoDT { get; set; }
        public string? DiaChi { get; set; }

        public string? TenDangNhap { get; set; }
        public string? MatKhau { get; set; }
        public string? XacNhanMatKhau { get; set; }
    }
}