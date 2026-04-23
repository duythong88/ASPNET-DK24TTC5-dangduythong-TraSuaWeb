


namespace TraSuaWeb.Models
{
    public class GioHangItem
    {
        public int MaSP { get; set; }
        public string? TenSP { get; set; }
        public string? HinhAnh { get; set; }

        public int MaSize { get; set; }
        public string? TenSize { get; set; }

        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public decimal ThanhTien
        {
            get { return DonGia * SoLuong; }
        }
    }
}