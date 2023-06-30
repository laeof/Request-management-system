using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
	public class RequestViewModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		public string? Comment { get; set; }

		public int Status { get; set; } = 1;
		[Required]
		public int Priority { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string CategoryId { get; set; }
		public string? ExecutorId { get; set; }

	}
}
