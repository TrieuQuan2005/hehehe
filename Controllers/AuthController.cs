using Microsoft.AspNetCore.Mvc;
using hehehe.Data;
using hehehe.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace hehehe.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.MaNhapHoc == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("MaNhapHoc", user.MaNhapHoc);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                return user.IsAdmin
                    ? RedirectToAction("Index", "Admin")
                    : RedirectToAction("FormNhapThongTin", "Form");
            }

            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu.";
            return View();
        }
    }
}