using hehehe.Data;
using hehehe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace hehehe.Controllers
{
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public FormController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public IActionResult FormNhapThongTin()
        {
            var maNhapHoc = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(maNhapHoc))
                return RedirectToAction("Login", "Auth");

            var prefix = maNhapHoc.Substring(0, 2).ToUpperInvariant();
            var form = _db.StudentForms.FirstOrDefault(f => f.MaNhapHoc == maNhapHoc);

            ViewBag.Department = prefix switch
            {
                "AT" => "An toàn thông tin",
                "CT" => "Công nghệ thông tin",
                "DT" => "Điện tử vi mạch",
                _ => "Không xác định"
            };

            if (form?.IsLocked == true)
                return View("XemThongTin", form);

            return View("FormNhapThongTin", form ?? new UserForm());
        }

        [HttpPost]
        public async Task<IActionResult> FormNhapThongTin(UserForm model, IFormFile uploadedFile)
        {
            var maNhapHoc = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(maNhapHoc))
                return RedirectToAction("Login", "Auth");

            var existing = _db.StudentForms.FirstOrDefault(f => f.MaNhapHoc == maNhapHoc);
            if (existing?.IsLocked == true)
                return View("Locked");

            string? filePath = existing?.UploadedFilePath;

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var userFolder = Path.Combine("uploads", maNhapHoc);
                var absoluteFolderPath = Path.Combine(_env.WebRootPath, userFolder);

                Directory.CreateDirectory(absoluteFolderPath);

                var ext = Path.GetExtension(uploadedFile.FileName);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
                var fileName = $"{maNhapHoc}_{timestamp}{ext}";
                var fileFullPath = Path.Combine(absoluteFolderPath, fileName);

                using var stream = new FileStream(fileFullPath, FileMode.Create);
                await uploadedFile.CopyToAsync(stream);

                filePath = $"/{userFolder.Replace("\\", "/")}/{fileName}";
            }

            if (existing == null)
            {
                var newForm = new UserForm
                {
                    MaNhapHoc = maNhapHoc,
                    HoTen = model.HoTen,
                    NgaySinh = model.NgaySinh,
                    DiemThi = model.DiemThi,
                    UploadedFilePath = filePath,
                    IsLocked = false
                };
                _db.StudentForms.Add(newForm);
            }
            else
            {
                existing.HoTen = model.HoTen;
                existing.NgaySinh = model.NgaySinh;
                existing.DiemThi = model.DiemThi;
                existing.UploadedFilePath = filePath;
                _db.StudentForms.Update(existing);
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Đã lưu thông tin thành công!";
            return RedirectToAction("FormNhapThongTin");
        }

        public IActionResult Locked()
        {
            return Content("Bạn không còn quyền chỉnh sửa. Thông tin đã bị khóa.");
        }
    }
}
