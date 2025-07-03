using Microsoft.AspNetCore.Mvc;
using hehehe.Data;
using hehehe.Models;
using hehehe.Models.ViewModels; 
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using hehehe.Services;
using ClosedXML.Excel;

namespace hehehe.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly EmailService _email;

        public AdminController(ApplicationDbContext db, EmailService email)
        {
            _db = db;
            _email = email;
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
            var allForms = _db.StudentForms.Where(f => f.MaNhapHoc != "admin").ToList();

            var totalUsers = _db.Users.Count() - 1;
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
            ViewBag.PendingCount = _db.YeuCauDinhChinhs.Count(x => !x.DaDuyet && !x.BiTuChoi);

            return View();
        }

        [HttpPost]
        public IActionResult ToggleLock(string id)
        {
            if (!IsAdmin()) return Unauthorized();

            var form = _db.StudentForms.Find(id);
            var studentInfos = _db.UserStdInfos.Find(id);
            if (form != null)
            {
                form.IsLocked = !form.IsLocked;
                _db.SaveChanges();
                
                var subject = "Thông báo về trạng thái form nhập thông tin";
                var acceptBody = $@"
                <p>Chào {WebUtility.HtmlEncode(form.HoTen)},</p>
                <p>Phòng Tuyển sinh học viện Kỹ thuật Mật Mã đã xác thực Phiếu sinh viên của bạn.</p>
                <p>Form của bạn hiện tại đã <strong>được xác thực</strong> và <strong>không thể</strong> chỉnh sửa.</p>
                <p>Chúc mừng bạn đã trở thành sinh viên của học viện kỹ thuật mật mã.</p>
                <p>Mã sinh viên của bạn là <strong>{studentInfos?.MaSinhVien}</strong></p>
                <p>Mật khẩu sinh viên của bạn là <strong>{studentInfos?.QlPassword}</strong>, vui lòng thay đổi mật khẩu sau lần đăng nhập đầu tiên</p>
                <p>Tài khoản thư viện của bạn là <strong>{studentInfos?.MsAccount}</strong></p>
                <p>Mật khẩu thư viện của bạn là <strong>{studentInfos?.MsPassword}</strong>, vui lòng thay đổi mật khẩu sau lần đăng nhập đầu tiên</p>
                <p>Chúc bạn có một kỳ học mới thành công tốt đẹp</p>
                <p>Nếu có thắc mắc hoặc muốn chỉnh sửa lại thông tin, vui lòng liên hệ với phòng tuyển sinh.</p>
                <p>Trân trọng.</p>";
                var rejectBody = $@"
                <p>Chào {WebUtility.HtmlEncode(form.HoTen)},</p>
                <p>Phòng Tuyển sinh học viện Kỹ thuật Mật Mã đã mở khóa Phiếu sinh viên của bạn.</p>
                <p>Form của bạn hiện tại đã <strong>có thể</strong> chỉnh sửa.</p>";
                var body = form.IsLocked ? acceptBody : rejectBody;

                if (!string.IsNullOrWhiteSpace(form.Email))
                    _email.SendEmail(form.Email, subject, body);
            }

            return RedirectToAction("Index");
        }

        public IActionResult XemChiTiet(string id)
        {
            var user = _db.Users.FirstOrDefault(u => u.MaNhapHoc == id && !u.IsAdmin);
            if (user == null) return NotFound();

            var form = _db.StudentForms.FirstOrDefault(f => f.MaNhapHoc == user.MaNhapHoc);
            if (form == null) return NotFound();

            return View("XemChiTiet", form);
        }

        [HttpGet]
        public IActionResult DuyetDinhChinh()
        {
            if (!IsAdmin()) return Unauthorized();

            var danhSach = _db.YeuCauDinhChinhs
                .OrderByDescending(x => x.NgayGui)
                .ToList();

            return View("DuyetDinhChinh",danhSach);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult DuyetYeuCau(int id)
        {
            if (!IsAdmin()) return Unauthorized();

            var yc = _db.YeuCauDinhChinhs.FirstOrDefault(x => x.Id == id);            
            if (yc == null) return NotFound();
            
            var form = _db.StudentForms.FirstOrDefault(f => f.MaNhapHoc == yc.MaNhapHoc);
            if (form == null) return NotFound("Không tìm thấy người dùng.");
            
            var init = _db.EduMinisUsers.FirstOrDefault(f => f.MaNhapHoc == yc.MaNhapHoc);
            var propInit = init.GetType().GetProperty(yc.TruongCanDinhChinh);
            var propForm = form.GetType().GetProperty(yc.TruongCanDinhChinh);
            if (propForm != null && propForm.CanWrite && propInit != null && propInit.CanWrite)
            {
                try
                {
                    object? newValue = Convert.ChangeType(yc.GiaTriMoi, propForm.PropertyType);
                    propForm.SetValue(form, newValue);
                    propInit.SetValue(init, newValue);
                    yc.DaDuyet = true;
                    yc.BiTuChoi = !yc.DaDuyet;
                    _db.SaveChanges();

                    string subject = "✅ Yêu cầu đính chính đã được duyệt";
                    string body = $@"
                        <p>Chào {WebUtility.HtmlEncode(form.HoTen)},</p>
                        <p>Yêu cầu đính chính trường <strong>{WebUtility.HtmlEncode(yc.TruongCanDinhChinh)}</strong> với giá trị mới <strong>{WebUtility.HtmlEncode(yc.GiaTriMoi)}</strong> đã được <span style='color:green;'>duyệt</span> bởi quản trị viên.</p>
                        <p>Thông tin trên đã được cập nhật trong hệ thống.</p>
                        <p>Trân trọng.</p>";

                    if (!string.IsNullOrWhiteSpace(form.Email))
                        _email.SendEmail(form.Email, subject, body);
                }
                catch (Exception)
                {
                    TempData["Error"] = "Không thể cập nhật giá trị mới do lỗi kiểu dữ liệu.";
                }
            }

            return RedirectToAction("DuyetDinhChinh");
        }

        [HttpPost]
        public IActionResult TuChoiYeuCau(int id, string GhiChuAdmin)
        {
            if (!IsAdmin()) return Unauthorized();

            var yc = _db.YeuCauDinhChinhs.FirstOrDefault(x => x.Id == id);
            if (yc == null) return NotFound();

            yc.BiTuChoi = true;
            yc.DaDuyet = false;
            yc.GhiChuAdmin = GhiChuAdmin;
            _db.SaveChanges();

            var form = _db.StudentForms.FirstOrDefault(f => f.MaNhapHoc == yc.MaNhapHoc);
            var user = _db.Users.FirstOrDefault(u => u.MaNhapHoc == yc.MaNhapHoc);

            string subject = "❌ Yêu cầu đính chính bị từ chối";
            string body = $@"
                <p>Chào {WebUtility.HtmlEncode(user?.MaNhapHoc ?? "thí sinh")},</p>
                <p>Yêu cầu đính chính trường <strong>{WebUtility.HtmlEncode(yc.TruongCanDinhChinh)}</strong> đã bị <span style='color:red;'>từ chối</span> bởi quản trị viên.</p>
                <p>Lý do từ chối: <em>{WebUtility.HtmlEncode(yc.GhiChuAdmin ?? "Không có ghi chú")}</em></p>
                <p>Nếu có thắc mắc, vui lòng liên hệ phòng tuyển sinh.</p>
                <p>Trân trọng.</p>";

            if (!string.IsNullOrWhiteSpace(form?.Email))
                _email.SendEmail(form.Email, subject, body);

            return RedirectToAction("DuyetDinhChinh");
        }
        
        [HttpGet]
        public IActionResult ExportExcelForm()
        {
            var nganhList = _db.StudentForms
                .Select(u => u.NganhDaoTao)
                .Distinct()
                .ToList();
            nganhList.Add("All");
            var model = new ExportOptionsViewModel
            {
                AvailableNganhs = nganhList
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ExportToExcel(string SelectedNganh)
        {
            
            var data = _db.StudentForms.ToList();
            if (SelectedNganh != "All") data = _db.StudentForms.Where(u => u.NganhDaoTao == SelectedNganh).ToList();
            
            
            

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Danh sách sinh viên");
                var currentRow = 1;
                
                // Header
                worksheet.Cell(currentRow, 1).Value = "STT";
                worksheet.Cell(currentRow, 2).Value = "Mã nhập học";
                worksheet.Cell(currentRow, 3).Value = "Họ tên";
                worksheet.Cell(currentRow, 4).Value = "Ngày sinh";
                worksheet.Cell(currentRow, 5).Value = "Giới tính";
                worksheet.Cell(currentRow, 6).Value = "Nơi sinh";
                worksheet.Cell(currentRow, 7).Value = "Dân tộc";
                worksheet.Cell(currentRow, 8).Value = "Nơi thường trú";
                worksheet.Cell(currentRow, 9).Value = "Chỗ ở hiện nay";
                worksheet.Cell(currentRow, 10).Value = "Đối tượng ưu tiên";
                worksheet.Cell(currentRow, 11).Value = "Khu vực";
                worksheet.Cell(currentRow, 12).Value = "Năm nhập học";
                worksheet.Cell(currentRow, 13).Value = "Năm tốt nghiệp THPT";
                worksheet.Cell(currentRow, 14).Value = "Năm nhập ngũ";
                worksheet.Cell(currentRow, 15).Value = "Năm xuất ngũ";
                worksheet.Cell(currentRow, 16).Value = "Ngày vào đoàn";
                worksheet.Cell(currentRow, 17).Value = "ngày vào Đảng";
                worksheet.Cell(currentRow, 18).Value = "Ngành đào tạo";
                worksheet.Cell(currentRow, 19).Value = "Số điện thoại";
                worksheet.Cell(currentRow, 20).Value = "Email";
                worksheet.Cell(currentRow, 21).Value = "Họ tên bố";
                worksheet.Cell(currentRow, 22).Value = "Tuổi bố";
                worksheet.Cell(currentRow, 23).Value = "Nghề nghiệp bố";
                worksheet.Cell(currentRow, 24).Value = "SDT bố";
                worksheet.Cell(currentRow, 25).Value = "Họ tên mẹ";
                worksheet.Cell(currentRow, 26).Value = "Tuổi mẹ";
                worksheet.Cell(currentRow, 27).Value = "Nghề nghiệp mẹ";
                worksheet.Cell(currentRow, 28).Value = "SDT mẹ";
                worksheet.Cell(currentRow, 29).Value = "Báo tin cho ai";
                worksheet.Cell(currentRow, 30).Value = "Địa chỉ liên hệ";
                
                
                // Dữ liệu
                foreach (var user in data)
                { 
                    worksheet.Columns().AdjustToContents();
                    currentRow++; 
                    worksheet.Cell(currentRow, 1).Value = currentRow-1;
                    worksheet.Cell(currentRow, 2).Value = user.MaNhapHoc;
                    worksheet.Cell(currentRow, 3).Value = user.HoTen;
                    worksheet.Cell(currentRow, 4).Value = user.NgaySinh.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 5).Value = user.GioiTinh;
                    worksheet.Cell(currentRow, 6).Value = user.NoiSinh;
                    worksheet.Cell(currentRow, 7).Value = user.DanToc;
                    worksheet.Cell(currentRow, 8).Value = user.NoiThuongTru;
                    worksheet.Cell(currentRow, 9).Value = user.ChoOHienNay;
                    worksheet.Cell(currentRow, 10).Value = user.DoiTuongUuTien  ? "Có" : "Không";
                    worksheet.Cell(currentRow, 11).Value = user.KhuVuc;
                    worksheet.Cell(currentRow, 12).Value = user.NamNhapHoc;
                    worksheet.Cell(currentRow, 13).Value = user.NamTotNghiepTHPT;
                    worksheet.Cell(currentRow, 14).Value = user.NamNhapNgu;
                    worksheet.Cell(currentRow, 15).Value = user.NamXuatNgu;
                    worksheet.Cell(currentRow, 16).Value = user.NgayVaoDoan;
                    worksheet.Cell(currentRow, 17).Value = user.NgayVaoDang;
                    worksheet.Cell(currentRow, 18).Value = user.NganhDaoTao;
                    worksheet.Cell(currentRow, 19).Value = user.SoDienThoai;    
                    worksheet.Cell(currentRow, 20).Value = user.Email;
                    worksheet.Cell(currentRow, 21).Value = user.HoTenBo;
                    worksheet.Cell(currentRow, 22).Value = user.TuoiBo;
                    worksheet.Cell(currentRow, 23).Value = user.NgheNghiepBo;
                    worksheet.Cell(currentRow, 24).Value = user.SoDienThoaiBo;
                    worksheet.Cell(currentRow, 25).Value = user.HoTenMe;
                    worksheet.Cell(currentRow, 26).Value = user.TuoiMe;
                    worksheet.Cell(currentRow, 27).Value = user.NgheNghiepMe;
                    worksheet.Cell(currentRow, 28).Value = user.SoDienThoaiMe;
                    worksheet.Cell(currentRow, 29).Value = user.BaoTinChoAi;
                    worksheet.Cell(currentRow, 30).Value = user.DiaChiLienHe;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"DanhSach_{SelectedNganh}_{DateTime.Now:yyyyMMdd}.xlsx");
                }
            }
        }

    }
}
