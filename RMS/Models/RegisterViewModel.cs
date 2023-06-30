using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public uint RoleId { get; set; }
        public string Comment { get; set; }

    }
}
