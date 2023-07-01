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
        [Required]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = "default firstname";
        [Required]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; } = "default surname";
        [Required]
        [Display(Name = "Логін")]
        public string Login { get; set; } = "login";
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = "password";

        [Display(Name = "Коментар")]
        public string? Comment { get; set; }
    }
}
