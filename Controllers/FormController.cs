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

            return View("FormNhapThongTin", form ?? new UserForm { MaNhapHoc = maNhapHoc });
        }

        [HttpPost]
        public async Task<IActionResult> FormNhapThongTin(UserForm model, IFormFile uploadedFile, string XacNhan)
        {
            var maNhapHoc = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(maNhapHoc))
                return RedirectToAction("Login", "Auth");
            
            if (XacNhan != "Yes")
            {
                TempData["Message"] = "Vui lòng xác nhận thông tin trước khi lưu.";
                return RedirectToAction("FormNhapThongTin");
            }   
            
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
                model.MaNhapHoc = maNhapHoc;
                model.UploadedFilePath = filePath;
                model.IsLocked = false;
                _db.StudentForms.Add(model);
            }
            else
            {
                existing.HoTen = model.HoTen;
                existing.NgaySinh = model.NgaySinh;
                existing.GioiTinh = model.GioiTinh;
                existing.NoiSinh = model.NoiSinh;
                existing.DanToc = model.DanToc;
                existing.NoiThuongTru = model.NoiThuongTru;
                existing.ChoOHienNay = model.ChoOHienNay;
                existing.DoiTuongUuTien = model.DoiTuongUuTien;
                existing.KhuVuc = model.KhuVuc;
                existing.NamNhapHoc = model.NamNhapHoc;
                existing.NamTotNghiepTHPT = model.NamTotNghiepTHPT;
                existing.NamNhapNgu = model.NamNhapNgu;
                existing.NamXuatNgu = model.NamXuatNgu;
                existing.NgayVaoDoan = model.NgayVaoDoan;
                existing.NgayVaoDang = model.NgayVaoDang;
                existing.NganhDaoTao = model.NganhDaoTao;
                existing.SoDienThoai = model.SoDienThoai;
                existing.Email = model.Email;
                existing.HoTenBo = model.HoTenBo;
                existing.TuoiBo = model.TuoiBo;
                existing.NgheNghiepBo = model.NgheNghiepBo;
                existing.SoDienThoaiBo = model.SoDienThoaiBo;
                existing.HoTenMe = model.HoTenMe;
                existing.TuoiMe = model.TuoiMe;
                existing.NgheNghiepMe = model.NgheNghiepMe;
                existing.SoDienThoaiMe = model.SoDienThoaiMe;
                existing.BaoTinChoAi = model.BaoTinChoAi;
                existing.DiaChiLienHe = model.DiaChiLienHe;
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
