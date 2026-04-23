using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaWeb.Models;

namespace TraSuaWeb.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly TraSuaDbContext _context;

        public TaiKhoanController(TraSuaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(DangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.MatKhau != model.XacNhanMatKhau)
                {
                    ViewBag.ThongBao = "Mật khẩu xác nhận không đúng.";
                    return View(model);
                }

                var taiKhoanDaTonTai = _context.TaiKhoans
                    .FirstOrDefault(tk => tk.TenDangNhap == model.TenDangNhap);

                if (taiKhoanDaTonTai != null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập đã tồn tại.";
                    return View(model);
                }

                var khachHang = new KhachHang
                {
                    TenKH = model.TenKH,
                    Email = model.Email,
                    SoDT = model.SoDT,
                    DiaChi = model.DiaChi
                };

                _context.KhachHangs.Add(khachHang);
                _context.SaveChanges();

                var taiKhoan = new TaiKhoan
                {
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau,
                    LoaiTaiKhoan = "Khach",
                    MaKH = khachHang.MaKH
                };

                _context.TaiKhoans.Add(taiKhoan);
                _context.SaveChanges();

                return RedirectToAction("DangNhap");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DangNhap()
        {
            return View("~/Views/TaiKhoan/DangNhap.cshtml");
        }

        [HttpPost]
        public IActionResult DangNhap(DangNhapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = _context.TaiKhoans
                    .Include(t => t.KhachHang)
                    .FirstOrDefault(t => t.TenDangNhap == model.TenDangNhap
                                      && t.MatKhau == model.MatKhau);

                if (taiKhoan == null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    return View("~/Views/TaiKhoan/DangNhap.cshtml", model);
                }

                HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap ?? "");
                HttpContext.Session.SetString("LoaiTaiKhoan", taiKhoan.LoaiTaiKhoan ?? "");
                HttpContext.Session.SetString("TenKH", taiKhoan.KhachHang?.TenKH ?? "");
                HttpContext.Session.SetInt32("MaKH", taiKhoan.MaKH);

                var maSp = HttpContext.Session.GetInt32("SanPhamDangChon");
                if (maSp != null)
                {
                    return RedirectToAction("Index", "SanPham");
                }

                if (taiKhoan.LoaiTaiKhoan == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/TaiKhoan/DangNhap.cshtml", model);
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
