using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace hehehe.Models
{
    public class User
    {
        [Key]
        public string MaNhapHoc { get; set; } = string.Empty;

        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();

        public string Password { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = false;

        public bool IsAdmin { get; set; } = false;

        public UserForm? UserForm { get; set; }
        
        public InitUserForm? InitUserForm { get; set; }

        public UserStdInfo? UserStdInfo { get; set; }
    }
}