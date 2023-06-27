using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
	public class LifecycleModel
	{
		// ID 
		public uint Id { get; set; }
		// Дата відкриття
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime Opened { get; set; }
		// Дата обробки
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Proccesing { get; set; }
		// Дата закриття
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Closed { get; set; }
		// Дата відміни
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Cancelled { get; set; }
	}
}
