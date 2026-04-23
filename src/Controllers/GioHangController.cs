using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaWeb.Models;
using TraSuaWeb.Models;

namespace TraSuaWeb.Controllers
{
    public class GioHangController : Controller
    {
        private readonly TraSuaDbContext _context;

        public GioHangController(TraSuaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");
            if (gioHang == null)
            {
                gioHang = new List<GioHangItem>();
            }

            return View(gioHang);
        }

        [HttpPost]
        public IActionResult ThemVaoGio(int maSp, int maSize, int soLuong)
        {
            var size = _context.SizeSanPhams
                .Include(s => s.SanPham)
                .FirstOrDefault(s => s.MaSize == maSize);

            if (size == null)
            {
                TempData["ThongBao"] = "Size sản phẩm không hợp lệ.";
                return RedirectToAction("Index", "SanPham");
            }

            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");
            if (gioHang == null)
            {
                gioHang = new List<GioHangItem>();
            }

            var sanPhamDaCo = gioHang.FirstOrDefault(x => x.MaSP == size.MaSP && x.MaSize == size.MaSize);

            if (sanPhamDaCo != null)
            {
                sanPhamDaCo.SoLuong += soLuong;
            }
            else
            {
                gioHang.Add(new GioHangItem
                {
                    MaSP = size.MaSP,
                    TenSP = size.SanPham?.TenSP,
                    HinhAnh = size.SanPham?.HinhAnh,
                    MaSize = size.MaSize,
                    TenSize = size.TenSize,
                    DonGia = size.DonGia,
                    SoLuong = soLuong
                });
            }

            HttpContext.Session.SetObjectAsJson("GioHang", gioHang);

            return RedirectToAction("Index");
        }

       

        public IActionResult TangSoLuong(int maSp, int maSize)
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");

            if (gioHang != null)
            {
                var sanPham = gioHang.FirstOrDefault(x => x.MaSP == maSp && x.MaSize == maSize);
                if (sanPham != null)
                {
                    sanPham.SoLuong += 1;
                    HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult GiamSoLuong(int maSp, int maSize)
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");

            if (gioHang != null)
            {
                var sanPham = gioHang.FirstOrDefault(x => x.MaSP == maSp && x.MaSize == maSize);
                if (sanPham != null)
                {
                    sanPham.SoLuong -= 1;

                    if (sanPham.SoLuong <= 0)
                    {
                        gioHang.Remove(sanPham);
                    }

                    HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CapNhatSoLuong(int maSp, int maSize, int soLuong)
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");

            if (gioHang != null)
            {
                var sanPham = gioHang.FirstOrDefault(x => x.MaSP == maSp && x.MaSize == maSize);
                if (sanPham != null)
                {
                    if (soLuong <= 0)
                    {
                        gioHang.Remove(sanPham);
                    }
                    else
                    {
                        sanPham.SoLuong = soLuong;
                    }

                    HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult XoaKhoiGio(int maSp, int maSize)
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");

            if (gioHang != null)
            {
                var sanPham = gioHang.FirstOrDefault(x => x.MaSP == maSp && x.MaSize == maSize);
                if (sanPham != null)
                {
                    gioHang.Remove(sanPham);
                    HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult DatHang()
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangItem>>("GioHang");

            if (gioHang == null || gioHang.Count == 0)
            {
                TempData["ThongBao"] = "Giỏ hàng đang trống.";
                return RedirectToAction("Index");
            }

            int? maKH = HttpContext.Session.GetInt32("MaKH");

            if (maKH == null)
            {
                TempData["ThongBaoDangNhap"] = "🚚 Hệ thống chỉ nhận giao hàng tại khu vực Đặc khu Phú Quý. Anh/chị vui lòng đăng nhập hoặc đăng ký tài khoản để đặt hàng.";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            decimal tongTien = gioHang.Sum(x => x.ThanhTien);

            var donHang = new DonHang
            {
                MaKH = maKH.Value,
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Chờ xác nhận",
                ThanhToan = "Chờ thanh toán"
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var item in gioHang)
            {
                var chiTiet = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSP = item.MaSP,
                    MaSize = item.MaSize,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                    ThanhTien = item.ThanhTien
                };

                _context.ChiTietDonHangs.Add(chiTiet);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("GioHang");
            TempData["ThongBao"] = "Đặt hàng thành công.";

            return RedirectToAction("Index");
        }
    }
}