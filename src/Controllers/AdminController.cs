using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaWeb.Models;
using Microsoft.AspNetCore.Hosting;

namespace TraSuaWeb.Controllers
{

    public class AdminController : Controller
    {
        private readonly TraSuaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(TraSuaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SanPham()
        {
            var danhSach = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .ToList();

            return View(danhSach);
        }

        [HttpGet]
        public IActionResult ThemSanPham()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ThemSanPham(SanPhamViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? fileName = null;

                if (model.FileHinh != null && model.FileHinh.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.FileHinh.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.FileHinh.CopyTo(fileStream);
                    }
                }

                var sanPham = new SanPham
                {
                    TenSP = model.TenSP,
                    MaDanhMuc = model.MaDanhMuc,
                    MoTa = model.MoTa,
                    HinhAnh = fileName != null ? "/images/" + fileName : null
                };

                _context.SanPhams.Add(sanPham);
                _context.SaveChanges();

                var size1 = new SizeSanPham
                {
                    TenSize = model.TenSize1,
                    DonGia = model.DonGia1,
                    MaSP = sanPham.MaSP
                };

                var size2 = new SizeSanPham
                {
                    TenSize = model.TenSize2,
                    DonGia = model.DonGia2,
                    MaSP = sanPham.MaSP
                };

                _context.SizeSanPhams.Add(size1);
                _context.SizeSanPhams.Add(size2);
                _context.SaveChanges();

                return RedirectToAction("SanPham");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult SuaSanPham(int id)
        {
            var sanPham = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .FirstOrDefault(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var model = new SanPhamViewModel
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                MaDanhMuc = sanPham.MaDanhMuc,
                MoTa = sanPham.MoTa,
                HinhAnh = sanPham.HinhAnh
            };

            if (sanPham.SizeSanPhams != null && sanPham.SizeSanPhams.Count > 0)
            {
                var size1 = sanPham.SizeSanPhams.ElementAtOrDefault(0);
                var size2 = sanPham.SizeSanPhams.ElementAtOrDefault(1);

                if (size1 != null)
                {
                    model.MaSize1 = size1.MaSize;
                    model.TenSize1 = size1.TenSize;
                    model.DonGia1 = size1.DonGia;
                }

                if (size2 != null)
                {
                    model.MaSize2 = size2.MaSize;
                    model.TenSize2 = size2.TenSize;
                    model.DonGia2 = size2.DonGia;
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SuaSanPham(SanPhamViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sanPham = _context.SanPhams
                    .Include(sp => sp.SizeSanPhams)
                    .FirstOrDefault(sp => sp.MaSP == model.MaSP);

                if (sanPham == null)
                {
                    return NotFound();
                }

                sanPham.TenSP = model.TenSP;
                sanPham.MaDanhMuc = model.MaDanhMuc;
                sanPham.MoTa = model.MoTa;

                if (model.FileHinh != null && model.FileHinh.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.FileHinh.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.FileHinh.CopyTo(fileStream);
                    }

                    sanPham.HinhAnh = "/images/" + fileName;
                }

                var size1 = sanPham.SizeSanPhams.FirstOrDefault(s => s.MaSize == model.MaSize1);
                var size2 = sanPham.SizeSanPhams.FirstOrDefault(s => s.MaSize == model.MaSize2);

                if (size1 != null)
                {
                    size1.TenSize = model.TenSize1;
                    size1.DonGia = model.DonGia1;
                }

                if (size2 != null)
                {
                    size2.TenSize = model.TenSize2;
                    size2.DonGia = model.DonGia2;
                }

                _context.SaveChanges();

                return RedirectToAction("SanPham");
            }

            return View(model);
        }


        public IActionResult XoaSanPham(int id)
        {
            var sanPham = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .FirstOrDefault(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            if (sanPham.SizeSanPhams != null && sanPham.SizeSanPhams.Any())
            {
                _context.SizeSanPhams.RemoveRange(sanPham.SizeSanPhams);
            }

            _context.SanPhams.Remove(sanPham);
            _context.SaveChanges();

            return RedirectToAction("SanPham");
        }
        public IActionResult DonHang()
        {
            var danhSachDonHang = _context.DonHangs
                .Include(dh => dh.KhachHang)
                .OrderByDescending(dh => dh.NgayDat)
                .ToList();

            return View(danhSachDonHang);
        }

        [HttpGet]
        public IActionResult OrderTaiQuay()
        {
            // Lấy danh sách món đang order (Session)
            var dsMon = HttpContext.Session.GetObjectFromJson<List<OrderTaiQuayItem>>("OrderTaiQuay");
            if (dsMon == null)
            {
                dsMon = new List<OrderTaiQuayItem>();
            }

            // Lấy khách hàng đã chọn trước đó (nếu có)
            int? maKHDaChon = HttpContext.Session.GetInt32("MaKHTaiQuay");

            var model = new OrderTaiQuayViewModel
            {
                MaKH = maKHDaChon ?? 0,

                DanhSachKhachHang = _context.KhachHangs
                    .Select(kh => new SelectListItem
                    {
                        Value = kh.MaKH.ToString(),
                        Text = kh.TenKH
                    }).ToList(),

                DanhSachSanPham = _context.SanPhams
                    .Select(sp => new SelectListItem
                    {
                        Value = sp.MaSP.ToString(),
                        Text = sp.TenSP
                    }).ToList(),

                DanhSachSize = new List<SelectListItem>(),
                DanhSachMon = dsMon
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult ThemMonTaiQuay(OrderTaiQuayViewModel model)
        {

            HttpContext.Session.SetInt32("MaKHTaiQuay", model.MaKH);
            var size = _context.SizeSanPhams
                .Include(s => s.SanPham)
                .FirstOrDefault(s => s.MaSize == model.MaSize);

            if (size == null)
            {
                TempData["ThongBaoLoi"] = "Size sản phẩm không hợp lệ.";
                return RedirectToAction("OrderTaiQuay");
            }

            var dsMon = HttpContext.Session.GetObjectFromJson<List<OrderTaiQuayItem>>("OrderTaiQuay");
            if (dsMon == null)
            {
                dsMon = new List<OrderTaiQuayItem>();
            }

            var monDaCo = dsMon.FirstOrDefault(x => x.MaSP == size.MaSP && x.MaSize == size.MaSize);

            if (monDaCo != null)
            {
                monDaCo.SoLuong += model.SoLuong;
            }
            else
            {
                dsMon.Add(new OrderTaiQuayItem
                {
                    MaSP = size.MaSP,
                    TenSP = size.SanPham?.TenSP,
                    MaSize = size.MaSize,
                    TenSize = size.TenSize,
                    DonGia = size.DonGia,
                    SoLuong = model.SoLuong
                });
            }

            HttpContext.Session.SetObjectAsJson("OrderTaiQuay", dsMon);
            TempData["ThongBao"] = "Đã thêm món vào đơn tại quầy.";

            return RedirectToAction("OrderTaiQuay");
        }
        public IActionResult XoaMonTaiQuay(int maSp, int maSize)
        {
            var dsMon = HttpContext.Session.GetObjectFromJson<List<OrderTaiQuayItem>>("OrderTaiQuay");

            if (dsMon != null)
            {
                var mon = dsMon.FirstOrDefault(x => x.MaSP == maSp && x.MaSize == maSize);
                if (mon != null)
                {
                    dsMon.Remove(mon);
                    HttpContext.Session.SetObjectAsJson("OrderTaiQuay", dsMon);
                }
            }

            return RedirectToAction("OrderTaiQuay");
        }
        [HttpPost]
        public IActionResult TaoDonTaiQuay(int maKH)
        {
            var dsMon = HttpContext.Session.GetObjectFromJson<List<OrderTaiQuayItem>>("OrderTaiQuay");

            if (dsMon == null || dsMon.Count == 0)
            {
                TempData["ThongBaoLoi"] = "Chưa có món nào trong đơn.";
                return RedirectToAction("OrderTaiQuay");
            }

            decimal tongTien = dsMon.Sum(x => x.ThanhTien);

            var donHang = new DonHang
            {
                MaKH = maKH,
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Chờ xác nhận",
                ThanhToan = "Chờ thanh toán",
                LoaiDonHang = "Tại quầy"
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var item in dsMon)
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

            HttpContext.Session.Remove("OrderTaiQuay");
            TempData["ThongBao"] = "Tạo đơn tại quầy thành công.";

            return RedirectToAction("DonHang");
        }

        [HttpGet]
        public JsonResult LaySizeTheoSanPham(int maSp)
        {
            var dsSize = _context.SizeSanPhams
                .Where(s => s.MaSP == maSp)
                .Select(s => new
                {
                    maSize = s.MaSize,
                    tenSize = s.TenSize,
                    donGia = s.DonGia
                }).ToList();

            return Json(dsSize);
        }

        public IActionResult ChiTietDonHang(int id)
        {
            var chiTiet = _context.ChiTietDonHangs
                .Include(ct => ct.SanPham)
                .Include(ct => ct.SizeSanPham)
                .Where(ct => ct.MaDonHang == id)
                .ToList();

            ViewBag.MaDonHang = id;
            return View(chiTiet);
        }
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var donHang = _context.DonHangs.FirstOrDefault(dh => dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            donHang.TrangThai = trangThai;
            _context.SaveChanges();

            return RedirectToAction("DonHang");
        }
        public IActionResult CapNhatThanhToan(int id)
        {
            var donHang = _context.DonHangs.FirstOrDefault(dh => dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            donHang.ThanhToan = "Đã thanh toán";
            _context.SaveChanges();

            return RedirectToAction("DonHang");
        }
        public IActionResult KhachHang()
        {
            var danhSachKhachHang = _context.KhachHangs
                .Include(kh => kh.TaiKhoans)
                .OrderByDescending(kh => kh.MaKH)
                .ToList();

            return View(danhSachKhachHang);
        }
        [HttpPost]
        public IActionResult XoaKhachHang(int id)
        {
            var khachHang = _context.KhachHangs
                .Include(kh => kh.TaiKhoans)
                .Include(kh => kh.DonHangs)
                .FirstOrDefault(kh => kh.MaKH == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            if (khachHang.DonHangs != null && khachHang.DonHangs.Any())
            {
                TempData["ThongBaoLoi"] = "Không thể xóa khách hàng này vì đã có đơn hàng.";
                return RedirectToAction("KhachHang");
            }

            if (khachHang.TaiKhoans != null && khachHang.TaiKhoans.Any())
            {
                _context.TaiKhoans.RemoveRange(khachHang.TaiKhoans);
            }

            _context.KhachHangs.Remove(khachHang);
            _context.SaveChanges();

            TempData["ThongBao"] = "Xóa khách hàng thành công.";
            return RedirectToAction("KhachHang");
        }

        public IActionResult DoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            var homNay = DateTime.Today;
            var dauThang = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var donHangHopLe = _context.DonHangs
     .Include(dh => dh.KhachHang)
     .Where(dh => dh.TrangThai == "Đã giao" && dh.ThanhToan == "Đã thanh toán");

            var model = new DoanhThuViewModel
            {
                DoanhThuHomNay = donHangHopLe
                    .Where(dh => dh.NgayDat.Date == homNay)
                    .Sum(dh => (decimal?)dh.TongTien) ?? 0,

                DoanhThuThangNay = donHangHopLe
                    .Where(dh => dh.NgayDat >= dauThang)
                    .Sum(dh => (decimal?)dh.TongTien) ?? 0,

                TongDonHang = donHangHopLe.Count(),

                TuNgay = tuNgay,
                DenNgay = denNgay
            };

            if (tuNgay.HasValue && denNgay.HasValue)
            {
                var denNgayCuoi = denNgay.Value.Date.AddDays(1).AddTicks(-1);

                model.DoanhThuTheoKhoang = donHangHopLe
                    .Where(dh => dh.NgayDat >= tuNgay.Value.Date && dh.NgayDat <= denNgayCuoi)
                    .Sum(dh => (decimal?)dh.TongTien) ?? 0;

                model.DanhSachDonHang = donHangHopLe
                    .Where(dh => dh.NgayDat >= tuNgay.Value.Date && dh.NgayDat <= denNgayCuoi)
                    .OrderByDescending(dh => dh.NgayDat)
                    .ToList();
            }
            else
            {
                model.DanhSachDonHang = donHangHopLe
                    .OrderByDescending(dh => dh.NgayDat)
                    .ToList();
            }

            return View(model);
        }
    }
}