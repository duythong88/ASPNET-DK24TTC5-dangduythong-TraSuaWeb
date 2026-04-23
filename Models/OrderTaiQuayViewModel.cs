using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraSuaWeb.Models;

namespace TraSuaWeb.Models
{
    public class OrderTaiQuayViewModel
    {
        public int MaKH { get; set; }

        public int MaSP { get; set; }
        public int MaSize { get; set; }
        public int SoLuong { get; set; }

        public List<SelectListItem> DanhSachKhachHang { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DanhSachSanPham { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DanhSachSize { get; set; } = new List<SelectListItem>();

        public List<OrderTaiQuayItem> DanhSachMon { get; set; } = new List<OrderTaiQuayItem>();
    }
}
