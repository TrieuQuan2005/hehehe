using hehehe.Data;
using hehehe.Models;
using hehehe.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
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
            var ma = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(ma)) return RedirectToAction("Login", "Auth");
            
            var initData = _db.EduMinisUsers.FirstOrDefault(x => x.MaNhapHoc == ma);
            if (initData == null) return NotFound("Không tìm thấy dữ liệu ban đầu.");

            var formData = _db.StudentForms.FirstOrDefault(x => x.MaNhapHoc == ma);

            if(formData?.IsLocked == true) return View("Locked", formData);
            
            var viewModel = new FormViewModel
            {
                InitData = initData,
                FormData = formData
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormNhapThongTin(UserForm model, List<IFormFile> uploadedFiles, IFormFile avatar, List<string>? FilesToDelete)
        {
            var ma = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(ma)) return RedirectToAction("Login", "Auth");
            var formData = _db.StudentForms.FirstOrDefault(x => x.MaNhapHoc == ma);
            if (formData != null && formData.IsLocked) return View("Locked", formData);
            
            var initData = _db.EduMinisUsers.FirstOrDefault(x => x.MaNhapHoc == ma);
            
            var savedFilePaths = new List<string>();
            var userFolder = Path.Combine("uploads", ma);
            var absolutePath = Path.Combine(_env.WebRootPath, userFolder);
            Directory.CreateDirectory(absolutePath);

            // Avatar
            if (avatar != null && avatar.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                const long maxFileSize = 5 * 1024 * 1024; // 5MB

                var ext = Path.GetExtension(avatar.FileName);
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("", "Ảnh đại diện không hợp lệ. Chỉ chấp nhận .jpg, .jpeg, .png, .pdf");
                    return View("FormNhapThongTin", new FormViewModel { InitData = initData, FormData = formData });
                }
                if (avatar.Length > maxFileSize)
                {
                    ModelState.AddModelError("", "Ảnh đại diện vượt quá dung lượng cho phép (5MB).");
                    return View("FormNhapThongTin", new FormViewModel { InitData = initData, FormData = formData });
                }
                
                var avatarFileName = $"{ma}_avatar{ext}";
                var fullPath = Path.Combine(absolutePath, avatarFileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await avatar.CopyToAsync(stream);

                var relativePath = $"/{userFolder.Replace("\\", "/")}/{avatarFileName}";
                savedFilePaths.Add(relativePath);
            }

            // Các file khác
            if (uploadedFiles != null && uploadedFiles.Count > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                const long maxFileSize = 5 * 1024 * 1024; // 5MB
                foreach (var file in uploadedFiles)
                {
                    if (file.Length > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        if (!allowedExtensions.Contains(ext))
                        {
                            ModelState.AddModelError("", $"File {file.FileName} có định dạng không hợp lệ.");
                            return View("FormNhapThongTin", new FormViewModel { InitData = initData, FormData = formData });
                        }
                        if (file.Length > maxFileSize)
                        {
                            ModelState.AddModelError("", $"File {file.FileName} vượt quá dung lượng cho phép (5MB).");
                            return View("FormNhapThongTin", new FormViewModel { InitData = initData, FormData = formData });
                        }
                        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        var fileName = $"{ma}_{timestamp}_{Guid.NewGuid()}{ext}";
                        var fullPath = Path.Combine(absolutePath, fileName);

                        using var stream = new FileStream(fullPath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        var relativePath = $"/{userFolder.Replace("\\", "/")}/{fileName}";
                        savedFilePaths.Add(relativePath);
                    }
                }
            }
            
            //xóa file
            if (formData != null)
            {
                model.MaNhapHoc = ma;

                if (FilesToDelete != null && FilesToDelete.Count > 0)
                {
                    formData.UploadedFiles = formData.UploadedFiles
                        .Where(f => !FilesToDelete.Contains(f))
                        .ToList();

                    foreach (var path in FilesToDelete)
                    {
                        var fullPath = Path.Combine(_env.WebRootPath, path.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                model.UploadedFiles = formData.UploadedFiles.Concat(savedFilePaths).ToList();

                _db.Entry(formData).CurrentValues.SetValues(model);
                _db.StudentForms.Update(formData);
            }

            // Lưu form
            if (formData == null)
            {
                model.MaNhapHoc = ma;
                model.UploadedFiles = savedFilePaths;
                model.IsLocked = false;
                //gan Thong Tin
                model.HoTen = initData.HoTen;
                model.NgaySinh = initData.NgaySinh;
                model.GioiTinh = initData.GioiTinh;
                model.NoiSinh = initData.NoiSinh;
                model.DanToc = initData.DanToc;
                model.NoiThuongTru = initData.NoiThuongTru;
                model.ChoOHienNay =initData.ChoOHienNay;
                model.NganhDaoTao = initData.NganhDaoTao;
                model.SoDienThoai = initData.SoDienThoai;
                model.Email = initData.Email;
                model.KhuVuc = initData.KhuVuc;

                _db.StudentForms.Add(model);
            }
            else
            {
                model.MaNhapHoc = ma;
                model.UploadedFiles = formData.UploadedFiles.Concat(savedFilePaths).ToList();
                model.IsLocked = false;
                
                //gan Thong Tin
                model.HoTen = initData.HoTen;
                model.NgaySinh = initData.NgaySinh;
                model.GioiTinh = initData.GioiTinh;
                model.NoiSinh = initData.NoiSinh;
                model.DanToc = initData.DanToc;
                model.NoiThuongTru = initData.NoiThuongTru;
                model.ChoOHienNay =initData.ChoOHienNay;
                model.NganhDaoTao = initData.NganhDaoTao;
                model.SoDienThoai = initData.SoDienThoai;
                model.Email = initData.Email;
                model.KhuVuc = initData.KhuVuc;

                _db.Entry(formData).CurrentValues.SetValues(model);
                formData.UploadedFiles = model.UploadedFiles;
                _db.StudentForms.Update(formData);
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Lưu thành công!";
            return View("Success", formData ?? model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuiYeuCauDinhChinh(YeuCauDinhChinh model, IFormFile? file)
        {
            if (!ModelState.IsValid) return RedirectToAction("FormNhapThongTin");

            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "MinhChung");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

                model.FileMinhChung = uniqueFileName;
            }

            model.NgayGui = DateTime.Now;
            model.DaDuyet = false;
            model.BiTuChoi = false;

            _db.YeuCauDinhChinhs.Add(model);
            _db.SaveChanges();

            TempData["Message"] = "📨 Yêu cầu đính chính đã được gửi!";
            return RedirectToAction("FormNhapThongTin");
        }

        [HttpGet]
        public IActionResult HocPhi()
        {
            var vm = new HocPhiViewModel();
            return View(vm);
        }
        
        //Xuat file doc
        /*[HttpGet]
        public IActionResult ExportToWord()
        {
            var id = HttpContext.Session.GetString("MaNhapHoc"); 
            var form = _db.StudentForms.FirstOrDefault(x => x.MaNhapHoc == id);
            if (form == null) return NotFound();

            string templatePath = Path.Combine(_env.WebRootPath, "Templates", "ThongTinSinhVien_Template.docx");
            string tempFilePath = Path.Combine(Path.GetTempPath(), $"ThongTin_{id}_{DateTime.Now.Ticks}.docx");
            
            using (var doc = DocX.Load(templatePath))
            {
                doc.ReplaceText("{{HoTen}}", form.HoTen ?? "");
                doc.ReplaceText("{{NgaySinh}}", form.NgaySinh.ToString("dd/MM/yyyy") ?? "");
                doc.ReplaceText("{{ThangSinh}}",form.NgaySinh.Month.ToString() ?? "");
                doc.ReplaceText("{{NamSinh}}", form.NgaySinh.Year.ToString() ?? "");
                doc.ReplaceText("{{GioiTinh}}", form.GioiTinh ?? "");
                doc.ReplaceText("{{NoiSinh}}", form.NoiSinh ?? "");
                doc.ReplaceText("{{DanToc}}", form.DanToc ?? "");
                doc.ReplaceText("{{NoiThuongTru}}", form.NoiThuongTru ?? "");
                doc.ReplaceText("{{ChoOHienNay}}", form.ChoOHienNay ?? "");
                doc.ReplaceText("{{DTUT}}", form.DoiTuongUuTien ? "Có" : "Không");
                doc.ReplaceText("{{KV}}", form.KhuVuc ?? "");
                doc.ReplaceText("{{NNH}}", form.NamNhapHoc.ToString() ?? "");
                doc.ReplaceText("{{NTN}}", form.NamTotNghiepTHPT.ToString() ?? "");
                doc.ReplaceText("{{NNN}}", form.NamNhapNgu.ToString() ?? "");
                doc.ReplaceText("{{NXN}}", form.NamXuatNgu.ToString() ?? "");
                doc.ReplaceText("{{Doan}}", form.NgayVaoDoan.ToString() ?? "");
                doc.ReplaceText("{{Dang}}", form.NgayVaoDang.ToString() ?? "");
                doc.ReplaceText("{{NganhDT}}", form.NganhDaoTao ?? "");
                doc.ReplaceText("{{SDT0}}", form.SoDienThoai ?? "");
                doc.ReplaceText("{{Email}}", form.Email ?? "");
                doc.ReplaceText("{{HoTenBo}}", form.HoTenBo ?? "");
                doc.ReplaceText("{{TuoiBo}}", form.TuoiBo.ToString() ?? "");
                doc.ReplaceText("{{NgheBo}}", form.NgheNghiepBo ?? "");
                doc.ReplaceText("{{SDT1}}", form.SoDienThoaiBo ?? "");
                doc.ReplaceText("{{HoTenMe}}", form.HoTenMe ?? "");
                doc.ReplaceText("{{TuoiMe}}", form.TuoiMe.ToString() ?? "");
                doc.ReplaceText("{{NgheMe}}", form.NgheNghiepMe ?? "");
                doc.ReplaceText("{{SDT2}}", form.SoDienThoaiMe ?? "");
                doc.ReplaceText("{{BaoTinChoAi}}", form.BaoTinChoAi ?? "");
                doc.ReplaceText("{{DiaChiLienHe}}", form.DiaChiLienHe ?? "");
                
                doc.SaveAs(tempFilePath);
            }

            var fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
            return File(fileBytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                $"ThongTin_{form.HoTen}_{DateTime.Now:yyyyMMdd}.docx");
        }*/
    }
}
