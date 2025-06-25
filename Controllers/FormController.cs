using hehehe.Data;
using hehehe.Models;
using hehehe.Services;
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
        private readonly CloudinaryService _cloudinary;

        public FormController(ApplicationDbContext db, IWebHostEnvironment env, CloudinaryService cloudinary)
        {
            _db = db;
            _env = env;
            _cloudinary = cloudinary;
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
                    return View("Locked", existing);
                return View(existing);
            }

            return View(new UserForm());
        }

        [HttpPost]
        public async Task<IActionResult> FormNhapThongTin(UserForm model, List<IFormFile> uploadedFiles, IFormFile avatar)
        {
            var ma = HttpContext.Session.GetString("MaNhapHoc");
            if (string.IsNullOrEmpty(ma))
                return RedirectToAction("Login", "Auth");

            var existing = _db.StudentForms.FirstOrDefault(x => x.MaNhapHoc == ma);
            if (existing != null && existing.IsLocked)
                return View("Locked", existing);

            var savedFileUrls = new List<string>();

            if (avatar != null && avatar.Length > 0)
            {
                var avatarUrl = await _cloudinary.UploadImageAsync(avatar, ma);
                savedFileUrls.Add(avatarUrl);
            }

            foreach (var file in uploadedFiles)
            {
                if (file.Length > 0)
                {
                    var fileUrl = await _cloudinary.UploadRawFileAsync(file, ma);
                    savedFileUrls.Add(fileUrl);
                }
            }
            
            if (existing == null)
            {
                model.MaNhapHoc = ma;
                model.UploadedFiles = savedFileUrls;
                model.IsLocked = false;
                _db.StudentForms.Add(model);
            }
            else
            {
                model.MaNhapHoc = ma;
                model.UploadedFiles = existing.UploadedFiles.Concat(savedFileUrls).ToList();
                model.IsLocked = false;

                _db.Entry(existing).CurrentValues.SetValues(model);
                existing.UploadedFiles = model.UploadedFiles;
                _db.StudentForms.Update(existing);
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Lưu thành công!";
            return View("Success", existing ?? model);
        }
    }
}
