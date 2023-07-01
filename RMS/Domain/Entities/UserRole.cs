using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities
{
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey("Key")]
        public uint UserRoleId { get; set; }
        [ForeignKey("Role")]
        public uint RoleId { get; set; }
        public Role? Role { get; set; }
        [ForeignKey("User")]
        public uint UserId { get; set; }
        public User? User { get; set; }
    }
}