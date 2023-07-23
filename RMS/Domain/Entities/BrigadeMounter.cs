using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Domain.Entities
{
    public class BrigadeMounter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }
        [Display(Name = "Id")]
        public uint BrigadeId { get; set; }
        [ForeignKey("BrigadeId")]
        public Brigade Brigade { get; set; }
        [Display(Name = "Виконувач")]
        public uint UserId { get; set; }
        [ForeignKey("UserId")]
        public User Mounter { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
