

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaWeb.Models;

namespace TraSuaWeb.Controllers
    {
    public class SanPhamController : Controller
    {
        private readonly TraSuaDbContext _context;

        public SanPhamController(TraSuaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var danhSach = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .ToList();

            return View(danhSach);
        }

        public IActionResult TraSua()
        {
            var danhSach = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .Where(sp => sp.MaDanhMuc == 1)
                .ToList();

            return View("Index", danhSach);
        }

        public IActionResult TraTraiCay()
        {
            var danhSach = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .Where(sp => sp.MaDanhMuc == 2)
                .ToList();

            return View("Index", danhSach);
        }
        public JsonResult LaySize(int id)
        {
            var sanPham = _context.SanPhams
                .Include(sp => sp.SizeSanPhams)
                .FirstOrDefault(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return Json(null);
            }

            var result = new
            {
                maSp = sanPham.MaSP,
                tenSp = sanPham.TenSP,
                hinhAnh = sanPham.HinhAnh,
                dsSize = sanPham.SizeSanPhams.Select(s => new
                {
                    maSize = s.MaSize,
                    tenSize = s.TenSize,
                    donGia = s.DonGia
                }).ToList()
            };

            return Json(result);
        }
        public IActionResult XoaSession()
        {
            HttpContext.Session.Remove("SanPhamDangChon");
            return Ok();
        }
    }

    }

