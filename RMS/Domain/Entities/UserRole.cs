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
        public uint RoleId { get; set; }
		[ForeignKey("RoleId")]
		public Role? Role { get; set; }
        public uint UserId { get; set; }
		[ForeignKey("UserId")]
		public User? User { get; set; }
    }
}