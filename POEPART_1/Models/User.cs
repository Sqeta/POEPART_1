namespace POEPART_1.Models
{
    public class User
    {
        // Primary Key
        public int userID { get; set; }

        public int employee_number { get; set; }


        // Full name of the lecturer/user
        public string full_names { get; set; }

        // Email address
        public string email_address { get; set; }

        // Password (you might hash this in real apps)
        public string password { get; set; }

        // Confirm password
        public string confirm_password { get; set; }

        // Role: Lecture, HR, Manager, etc.
        public string role { get; set; }
    }
}
