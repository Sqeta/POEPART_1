using System.ComponentModel.DataAnnotations;

namespace POEPART_1.Models
{
    public class Register
    {
        [Required] 
        public string full_names { get; set; }
        [Required]
        public string email_address { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirm_password { get; set; }
        [Required]
        public string role {  get; set; }   
    }
}
