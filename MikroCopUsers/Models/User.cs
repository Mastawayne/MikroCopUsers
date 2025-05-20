using System.ComponentModel.DataAnnotations;

namespace MikroCopUsers.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Language { get; set; }
        public string Culture { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}