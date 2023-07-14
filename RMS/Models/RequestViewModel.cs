using RMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class RequestViewModel
    {
        public uint? Id { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string? Name { get; set; }
        [Required]
		[Display(Name = "Опис")]
		public string? Description { get; set; }
        [Required]
		[Display(Name = "Коментар")]
		public string? Comment { get; set; }
        [Required]
        public int? Status { get; set; }
        [Required]
        public int? Priority { get; set; }
        [Required]
		[Display(Name = "Адреса")]
		public string? Address { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Request>? Requests { get; set; }
		[Required]
        [Display(Name = "Категорія")]
		public uint? CategoryId { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
