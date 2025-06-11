using System.ComponentModel.DataAnnotations;

namespace GmiBetting.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? Language { get; set; }
        public string? Culture { get; set; }
        public required string PasswordHash { get; set; }
    }
}