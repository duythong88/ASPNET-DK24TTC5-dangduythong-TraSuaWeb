namespace TraSuaWeb.Models
{
    public class OrderTaiQuayItem
    {
        public int MaSP { get; set; }
        public string? TenSP { get; set; }

        public int MaSize { get; set; }
        public string? TenSize { get; set; }

        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public decimal ThanhTien => DonGia * SoLuong;
    }
}