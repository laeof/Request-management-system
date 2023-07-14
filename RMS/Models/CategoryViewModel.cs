using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using RMS.Domain.Entities;

namespace RMS.Models
{
    public class CategoryViewModel
    {
        public uint Id { get; set; }
        [Required]
        [Display(Name="Назва")]
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Category> Categories { get; set; }
    }
}
