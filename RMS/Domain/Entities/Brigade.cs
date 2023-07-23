using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities
{
	public class Brigade
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public uint Id { get; set; }
        public List<BrigadeMounter>? BrigadeMounters { get; set; }
        public bool? IsDeleted { get; set; }
	}
}
