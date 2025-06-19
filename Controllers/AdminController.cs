using Microsoft.AspNetCore.Mvc;
using hehehe.Data;
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

        public IActionResult Index()
        {
            if (!IsAdmin()) return Unauthorized();

            // Lấy danh sách form đã nhập
            var allForms = _db.StudentForms
                .Include(f => f.User)
                .ToList();

            // Tách theo ngành dựa vào Mã nhập học
            var formsAT = allForms.Where(f => f.MaNhapHoc.StartsWith("AT", System.StringComparison.OrdinalIgnoreCase)).ToList();
            var formsCT = allForms.Where(f => f.MaNhapHoc.StartsWith("CT", System.StringComparison.OrdinalIgnoreCase)).ToList();
            var formsDT = allForms.Where(f => f.MaNhapHoc.StartsWith("DT", System.StringComparison.OrdinalIgnoreCase)).ToList();

            // Lấy tổng số tài khoản không phải admin
            int totalAccounts = _db.Users.Count(u => !u.IsAdmin);
            int atCount = formsAT.Count;
            int ctCount = formsCT.Count;
            int dtCount = formsDT.Count;

            // Truyền dữ liệu sang view
            ViewBag.AT = formsAT;
            ViewBag.CT = formsCT;
            ViewBag.DT = formsDT;

            ViewBag.Stats = new
            {
                TotalUsers = totalAccounts,
                FilledCount = atCount + ctCount + dtCount,
                AT = new { Count = atCount, Percent = totalAccounts > 0 ? (int)((double)atCount * 100 / totalAccounts) : 0 },
                CT = new { Count = ctCount, Percent = totalAccounts > 0 ? (int)((double)ctCount * 100 / totalAccounts) : 0 },
                DT = new { Count = dtCount, Percent = totalAccounts > 0 ? (int)((double)dtCount * 100 / totalAccounts) : 0 }
            };

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
