using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace RMS.Domain.Entities
{
    public class User
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
		[Display(Name = "Ім'я")]
		[Required]
        public string FirstName { get; set; } = "default firstname";
        [Required]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; } = "default surname";
        [Required]
        [Display(Name = "Логін")]
        public string Login { get; set; } = "login";
        [Required]
        [Display(Name = "Пароль")]
        public string Password
        {
            get 
            { 
                return password;
            }
            set
            {
                if(value != null)
					password = SecurePasswordHasher.Hash(value);
			}
		}
        private string password;

		[Display(Name = "Коментар")]
        public string? Comment { get; set; }
        public string? ImgPath { get; set; } = "../../img/Avatar/user.png";
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? WorkHours { get; set; }
        public ICollection<BrigadeMounter>? BrigadeMounters { get; set; }
		public ICollection<UserRole>? UserRoles { get; set; }
	}
}
