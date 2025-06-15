using Microsoft.AspNetCore.Mvc;
using hehehe.Data;
using hehehe.Models;
using Microsoft.AspNetCore.Http;

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
            // "username" chính là MaNhapHoc
            var user = _db.Users.FirstOrDefault(u => u.MaNhapHoc == username && u.Password == password);

            if (user != null)
            {
                // Lưu thông tin vào session (dùng MaNhapHoc thay vì Id)
                HttpContext.Session.SetString("MaNhapHoc", user.MaNhapHoc);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                if (user.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("FormNhapThongTin", "Form");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}