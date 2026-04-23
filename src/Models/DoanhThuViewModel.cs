namespace TraSuaWeb.Models
{
    public class DoanhThuViewModel
    {
        public decimal DoanhThuHomNay { get; set; }
        public decimal DoanhThuThangNay { get; set; }
        public int TongDonHang { get; set; }

        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

        public decimal DoanhThuTheoKhoang { get; set; }

        public List<DonHang> DanhSachDonHang { get; set; } = new List<DonHang>();
    }
}