using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace RMS.Models
{
	public class UserModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
		[Required]
		[Display(Name = "Ім'я")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Прізвище")]
		public string Surname { get; set; }
		[Required]
		[Display(Name = "Логін")]
		public string Login { get; set; }
		[Required]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Display(Name = "Коментар")]
		public string Comment { get; set; }
	}
}
