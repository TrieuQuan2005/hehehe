using hehehe.Data;
using hehehe.Models;
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
            if (string.IsNullOrEmpty(ma))
                return RedirectToAction("Login", "Auth");

            var existing = _db.StudentForms.FirstOrDefault(x => x.MaNhapHoc == ma);
            if (existing != null)
            {
                if (existing.IsLocked)
                    return View("Locked");
                return View(existing);
            }

            return View(new UserForm());
        }

        [HttpPost]
        public async Task<IActionResult> FormNhapThongTin(UserForm model, List<IFormFile> uploadedFiles)
        {
            var ma = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(ma))
                return RedirectToAction("Login", "Auth");
            
            var existing = _db.StudentForms.FirstOrDefault(x => x.MaNhapHoc == ma);
            if (existing != null && existing.IsLocked)
                return View("Locked");

            var savedFilePaths = new List<string>();

            if (uploadedFiles != null && uploadedFiles.Count > 0)
            {
                var userFolder = Path.Combine("uploads", ma);
                var absolutePath = Path.Combine(_env.WebRootPath, userFolder);
                Directory.CreateDirectory(absolutePath);

                foreach (var file in uploadedFiles)
                {
                    if (file.Length > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
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

            if (existing == null)
            {
                model.MaNhapHoc = ma;
                model.UploadedFiles = savedFilePaths;
                model.IsLocked = false;
                _db.StudentForms.Add(model);
            }
            else
            {
                model.MaNhapHoc = ma;
                model.UploadedFiles = existing.UploadedFiles.Concat(savedFilePaths).ToList();
                model.IsLocked = false;

                _db.Entry(existing).CurrentValues.SetValues(model);
                existing.UploadedFiles = model.UploadedFiles;
                _db.StudentForms.Update(existing);
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Lưu thành công!";
            return RedirectToAction("FormNhapThongTin");
        }
    }
}
