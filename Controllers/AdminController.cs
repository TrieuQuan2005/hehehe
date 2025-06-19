using Microsoft.AspNetCore.Mvc;
using hehehe.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

            var allForms = _db.StudentForms
                .Include(f => f.User)
                .ToList();

            var formsAT = allForms.Where(f => f.MaNhapHoc.StartsWith("AT", StringComparison.OrdinalIgnoreCase)).ToList();
            var formsCT = allForms.Where(f => f.MaNhapHoc.StartsWith("CT", StringComparison.OrdinalIgnoreCase)).ToList();
            var formsDT = allForms.Where(f => f.MaNhapHoc.StartsWith("DT", StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.AT = formsAT;
            ViewBag.CT = formsCT;
            ViewBag.DT = formsDT;

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