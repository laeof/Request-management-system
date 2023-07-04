using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
        public List<Category>? Categories { get; set; }
        // Зовнішній ключ
        // ID життєвого циклу заявки - звичайне
        [Display(Name = "Життєвий цикл")]
        public uint LifecycleId { get; set; }
        // Силка на життєвий цикл заявки - навігаційне
        public Lifecycle? Lifecycle { get; set; }
        // Зовнішній ключ
        // ID користувача - звичайне
        public uint? CloseId { get; set; }
        // користувач - навігаційне
        [Display(Name = "Ким закрито")]
        public User? Closed { get; set; }
        // Зовнішній ключ
        // ID користувача - звичайне
        public uint? CancelId { get; set; }
        // користувач - навігаційне
        [Display(Name = "Ким відмінено")]
        public User? Cancelled { get; set; }
        // Зовнішній ключ
        // ID користувача - звичайне
        public uint? OpenId { get; set; }
        // користувач - навігаційне
        [Display(Name = "Ким відкрито")]
        public User? Opened { get; set; }
        //користувач - звичайне
        public string CreatedName { get; set; } = "1";
        //користувач - навігаційне
        public User? Created { get; set; }
    }
}
