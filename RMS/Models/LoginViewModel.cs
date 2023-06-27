using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RMS.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "*   Поле обов'язкове")]
		[Display(Name = "Логін")]
		public string Login { get; set; }

		[Required(ErrorMessage = "*   Поле обов'язкове")]
		[UIHint("password")]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Display(Name = "Запам'ятати мене?")]
		public bool RememberMe { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
