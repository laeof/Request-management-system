using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }
        //Назва
        [Required(ErrorMessage = "Введіть назву заявки")]
        [Display(Name = "Назва заявки")]
        public string Name { get; set; } = "default name";
        // Опис
        [Required(ErrorMessage = "Введіть опис заявки")]
        [Display(Name = "Опис")]
        public string Description { get; set; } = "default descr";
        // Коментар
        [Display(Name = "Коментар")]
        public string Comment { get; set; } = "default coment";
        // Статус заявки
        [Display(Name = "Статус")]
        public int Status { get; set; } = 1;
        // Пріорітет заявки
        [Display(Name = "Пріорітет")]
        public int Priority { get; set; } = 1;
        // Адреса
        [Display(Name = "Адреса")]
        public string Address { get; set; } = "default address";
        // Зовнішній ключ Категорія
        [Display(Name = "Категорія")]
		[ForeignKey("Category")]
		public uint CategoryId { get; set; }
        public Category? Category { get; set; }
        // Зовнішній ключ
        // ID життєвого циклу заявки - звичайне
        [Display(Name = "Життєвий цикл")]
		[ForeignKey("Lifecycle")]
		public uint LifecycleId { get; set; } = 1;
		public Lifecycle? Lifecycle { get; set; }
		// Зовнішній ключ
		// ID закриваючого - звичайне
		[Display(Name = "Ким закрито")]
		[ForeignKey("Close")]
		public uint? ClosedId { get; set; }
		public User? Close { get; set; }
		// Зовнішній ключ
		// ID відмінюючого - звичайне
		[Display(Name = "Ким відмінено")]
		[ForeignKey("Cancel")]
		public uint? CancelledId { get; set; }
		public User? Cancel { get; set; }
		// Зовнішній ключ
		// ID відкриваючого - звичайне
		[Display(Name = "Ким відкрито")]
		[ForeignKey("Open")]
		public uint? OpenedId { get; set; }
        public User? Open { get; set; }
        //ID створювача - звичайне
        public uint CreatedId { get; set; } = 1;
        //чи видалено
        public bool? IsDeleted { get; set; } = false;
    }
}
