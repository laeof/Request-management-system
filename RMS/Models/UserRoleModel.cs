using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
	public class UserRoleModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey("Key")]
        public uint UserRoleId { get; set; }
        [ForeignKey("Role")]
        public uint RoleId { get; set; }
        public RoleModel Role { get; set; }
        [ForeignKey("User")]
        public uint UserId { get; set; }
		public UserModel User { get; set; }
	}
}