using RMS.Domain.Entities;

namespace RMS.Models
{
    public class UserViewModel
    {
        public UserRole? UserRole { get; set; }
        public List<UserRole>? UserRoles { get; set; }
    }
}
