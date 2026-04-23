using Microsoft.AspNetCore.Http;

namespace TraSuaWeb.Models
{
    public class SanPhamViewModel
    {
        public int MaSP { get; set; }

        public string? TenSP { get; set; }
        public int? MaDanhMuc { get; set; }
        public string? MoTa { get; set; }

        public string? HinhAnh { get; set; }
        public IFormFile? FileHinh { get; set; }

        public int MaSize1 { get; set; }
        public string? TenSize1 { get; set; }
        public decimal DonGia1 { get; set; }

        public int MaSize2 { get; set; }
        public string? TenSize2 { get; set; }
        public decimal DonGia2 { get; set; }
    }
}