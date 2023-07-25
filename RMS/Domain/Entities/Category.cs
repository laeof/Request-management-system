using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace RMS.Domain.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
        [Display(Name = "Назва категорії")]
        public string? Name { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
