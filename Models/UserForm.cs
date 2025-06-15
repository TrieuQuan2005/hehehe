using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hehehe.Models
{
    public class UserForm
    {
        [Key]
        [ForeignKey("User")]
        public string MaNhapHoc { get; set; } = string.Empty;

        public string HoTen { get; set; } = string.Empty;

        public DateTime NgaySinh { get; set; }

        public float DiemThi { get; set; }

        public string? UploadedFilePath { get; set; }

        public bool IsLocked { get; set; } = false;

        public User? User { get; set; }
    }
}