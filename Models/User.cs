using System.ComponentModel.DataAnnotations;

namespace hehehe.Models
{
    public class User
    {
        [Key]
        public string MaNhapHoc { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsAdmin { get; set; } = false;

        public UserForm? UserForm { get; set; }
    }
}