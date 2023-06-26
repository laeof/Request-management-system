using System.ComponentModel.DataAnnotations;

namespace RMS.Domain.Entities
{
	public abstract class EntityBase
	{
		protected EntityBase() => DateAdded = DateTime.UtcNow;
		[Required]
		public Guid Id { get; set; }
		[Display(Name = "Название (заголовок)")]
		public virtual string? Title { get; set; }
		[Display(Name ="Краткое описание")]
		public virtual string? Subtitle{ get; set; }
		[Display(Name = "Полное описание")]
		public virtual string? Text { get; set; }
		[Display(Name ="Титульная картика")]
		public virtual string? TitleImagePath { get; set; }
		[DataType(DataType.Time)]
		public DateTime DateAdded { get; set; }
	}
}
