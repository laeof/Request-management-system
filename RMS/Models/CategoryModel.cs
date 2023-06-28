using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace RMS.Models
{
	public class CategoryModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
		[Required]
		[Display(Name = "Назва категорії")]
		public string Name { get; set; }
	}
}
