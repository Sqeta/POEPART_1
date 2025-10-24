using System.ComponentModel.DataAnnotations;

namespace POEPART_1.Models
{
    public class Claim
    {
        public int Claim_Id { get; set; }

        [Required(ErrorMessage = "Employee Number is required")]
        public int EmployeeNum { get; set; }

        [Required(ErrorMessage = "Lecture Name is required")]
        public string Lecture_Name { get; set; }

        [Required(ErrorMessage = "Module Name is required")]
        public string Module_Name { get; set; }

        [Required(ErrorMessage = "Number of Sessions is required")]
        public int Number_ofSessions { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required")]
        public decimal Hourly_rate { get; set; }

        [Required]
        public decimal Total_Amount { get; set; }

        [Required(ErrorMessage = "Claim Start Date is required")]
        public DateTime Claim_Start_Date { get; set; }

        [Required(ErrorMessage = "Claim End Date is required")]
        public DateTime Claim_End_Date { get; set; }

        public string? Claim_Description { get; set; }

        [Required(ErrorMessage = "File is required")]
        public IFormFile SupportingFile { get; set; }

        public string? Claim_Status { get; set; } = "Pending";
    }
}
