using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
	public class LifecycleModel
	{
		// ID 
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }
		// Дата відкриття
		[Display(Name = "Дата відкриття")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime Opened { get; set; }
        // Дата обробки
        [Display(Name = "Дата обробки")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Proccesing { get; set; }
        // Дата закриття
        [Display(Name = "Дата закриття")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Closed { get; set; }
        // Дата відміни
        [Display(Name = "Дата відміни")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Cancelled { get; set; }
	}
}
