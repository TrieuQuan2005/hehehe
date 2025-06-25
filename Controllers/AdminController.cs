using Microsoft.AspNetCore.Mvc;
using hehehe.Data;
using hehehe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace hehehe.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin()
        {
            var maNhapHoc = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(maNhapHoc)) return false;

            var user = _db.Users.Find(maNhapHoc);
            return user != null && user.IsAdmin;
        }

        [HttpGet]
        public IActionResult Index(string nganh = "", string maNhapHoc = "")
        {
            var allForms = _db.StudentForms
                .Where(f => f.MaNhapHoc != "admin")
                .ToList();

            var totalUsers = _db.Users.Count() -1;
            var filledCount = allForms.Count;

            var atList = allForms.Where(f => f.NganhDaoTao == "An toàn thông tin").ToList();
            var ctList = allForms.Where(f => f.NganhDaoTao == "Công nghệ thông tin").ToList();
            var dtList = allForms.Where(f => f.NganhDaoTao == "Điện tử vi mạch").ToList();

            var stats = new
            {
                TotalUsers = totalUsers,
                FilledCount = filledCount,
                AT = new { Count = atList.Count, Percent = atList.Count * 100 / Math.Max(1, filledCount) },
                CT = new { Count = ctList.Count, Percent = ctList.Count * 100 / Math.Max(1, filledCount) },
                DT = new { Count = dtList.Count, Percent = dtList.Count * 100 / Math.Max(1, filledCount) }
            };

            ViewBag.Stats = stats;

            if (!string.IsNullOrEmpty(nganh))
                allForms = allForms.Where(f => f.NganhDaoTao == nganh).ToList();

            if (!string.IsNullOrEmpty(maNhapHoc))
                allForms = allForms.Where(f => f.MaNhapHoc.Contains(maNhapHoc)).ToList();

            ViewBag.NganhFilter = nganh;
            ViewBag.MaNhapHocFilter = maNhapHoc;
            ViewBag.List = allForms;

            return View();
        }

        
        [HttpPost]
        public IActionResult ToggleLock(string id)
        {
            if (!IsAdmin()) return Unauthorized();

            var form = _db.StudentForms.Find(id);
            if (form != null)
            {
                form.IsLocked = !form.IsLocked;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
