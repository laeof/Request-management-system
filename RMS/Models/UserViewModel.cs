using RMS.Domain.Entities;

namespace RMS.Models
{
    public class UserViewModel: UserRole
    {
        public List<UserRole>? UserRoles { get; set; }
    }
}
