using System.ComponentModel.DataAnnotations;

namespace POEPART_1.Models
{
    public class Login
    {
        [Required]
        public string email_address { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string role { get; set; }
    }
}
