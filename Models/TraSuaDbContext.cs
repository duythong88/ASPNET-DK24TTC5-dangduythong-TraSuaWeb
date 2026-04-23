

using Microsoft.EntityFrameworkCore;

namespace TraSuaWeb.Models
{
    public class TraSuaDbContext : DbContext
    {
        public TraSuaDbContext(DbContextOptions<TraSuaDbContext> options) : base(options)
        {
        }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<SizeSanPham> SizeSanPhams { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}