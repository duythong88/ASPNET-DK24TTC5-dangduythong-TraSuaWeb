using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TraSuaWeb.Models;
using System.Linq;

namespace TraSuaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly TraSuaDbContext _context;

        public HomeController(TraSuaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string tukhoa)
        {
            var danhSach = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .AsQueryable();
            if(!string.IsNullOrEmpty(tukhoa))
            {
                danhSach = danhSach.Where(sp => sp.TenSP.Contains(tukhoa));
                ViewBag.Tukhoa = tukhoa;
                return View(danhSach.ToList());
            }
            ViewBag.Tukhoa = tukhoa;
            
           return View(danhSach.Take(9).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GioiThieu()
        {
            return View();
        }

        public IActionResult LienHe()
        {
            return View();
        }

        public IActionResult DatHangNhanh(int id)
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                // 👉 LƯU sản phẩm lại
                HttpContext.Session.SetInt32("SanPhamDangChon", id);

                TempData["ThongBaoDangNhap"] =
                    "🚚 Chỉ giao hàng tại Đặc khu Phú Quý. Vui lòng đăng nhập để mua hàng.";

                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // 👉 Đã login → lưu lại để mở modal
            HttpContext.Session.SetInt32("SanPhamDangChon", id);

            return RedirectToAction("Index", "SanPham");
        }
        public IActionResult DangNhap()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}