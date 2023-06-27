using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
	public class RequestModel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
		//Назва
		[Required]
		[Display(Name = "Назва заявки")]
		public string Name { get; set; }
		// Опис
		[Required]
		[Display(Name = "Опис")]
		public string Description { get; set; }
		// Коментар
		[Display(Name = "Коментар")]
		public string Comment { get; set; }
		// Статус заявки
		[Display(Name = "Статус")]
		public int Status { get; set; }
		// Пріорітет заявки
		[Display(Name = "Пріорітет")]
		public int Priority { get; set; }

		// Файл ошибки
		[Display(Name = "Файл с ошибкой")]
		public string File { get; set; }

		// Внешний ключ Категорія
		[Display(Name = "Категорія")]
		public int? CategoryId { get; set; }
		public CategoryModel Category { get; set; }

		// Зовнішній ключ
		// ID Користувача - звичайне
		public int? UserId { get; set; }
		// Користувач - навігаційне
		public UserModel User { get; set; }

		// Зовнішній ключ
		// ID виконувача - звичайне
		public int? ExecutorId { get; set; }
		// Виконувач - навігаційне
		public UserModel Executor { get; set; }

		// Зовнішній ключ
		// ID життєвого циклу заявки - звичайне
		public int LifecycleId { get; set; }
		// Силка на життєвий цикл заявки - навігаційне
		public LifecycleModel Lifecycle { get; set; }

		// Статус заявки
		public enum RequestStatus
		{
			Open = 1,
			Proccesing = 2,
			Closed = 3,
			Cancelled = 4
		}
		// Пріорітет заявки
		public enum RequestPriority
		{
			Default = 1,
			Critical = 2
		}
	}
}
