namespace POEPART_1.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int LecturerId { get; set; }
        public string LecturerName { get; set; }
        public string MonthYear { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>();

        // make it settable
        // Computed property
        public decimal TotalAmount => Claims?.Sum(c => c.Total_Amount) ?? 0;
    }



}
