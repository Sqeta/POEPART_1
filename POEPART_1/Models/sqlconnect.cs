using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace POEPART_1.Models
{
    public class sqlconnect
    {
        private string connection = @"Server=(localDB)\monthly_claim_system;Database=claim_system;";



        public void create_table()
        {
            //try and catch for error handling
            try
            {
                //connect first to open the port using the using() function
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    //open connection 
                    connect.Open();
                    //Temp variable to hold query
                    string query = @"create table users(
                                   userID int identity(100,10) primary key, 
                                   full_names varchar(100) not null,
                                   email_address varchar(100) not null,
                                   password varchar(100) not null,
                                   confirm_password varchar(100) not null,
                                   role varchar(50)
                                   )";

                    //use the SQLCommand class to run the query 
                    using (SqlCommand create_table = new SqlCommand(query, connect))
                    {
                        //run the query 
                        create_table.ExecuteNonQuery();
                        //the shwo succes message
                        Console.WriteLine("Awezz PAPIKI!!!! Table is created");
                        //the close the connection
                        connect.Close();
                    }

                }
            }
            catch (Exception error)
            {
                //show error message
                Console.WriteLine(error.Message);
            }
        }

        public void store_user(string name,string email,string password,string confirm_password, string role)
        {
            //try and catch for error handling
            try
            {  
                //connect first to open the port using the using() function
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    //open connection 
                    connect.Open();
                    //Temp variable to hold query
                    string query = @"INSERT INTO users (full_names,email_address,password,confirm_password,role) VALUES
                    ('"+ name + "', '"+ email + "', '"+password+"', '"+confirm_password+"', '"+role+"')";

                    //use the SQLCommand class to run the query 
                    using (SqlCommand create_table = new SqlCommand(query, connect))
                    {
                        //run the query 
                        create_table.ExecuteNonQuery();
                        //the shwo succes message
                        Console.WriteLine("Awezz PAPIKI!!!! Data is stored");
                        //the close the connection
                        connect.Close();
                    }

                }
            }
            catch (Exception error)
            {
                //show error message
                Console.WriteLine(error.Message);
            }
        }

        public bool LoginUser(string email, string password, string role)
        {
            bool isfound = false;

            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    // Use parameters to prevent SQL injection
                    string query = @"SELECT COUNT(*) FROM users 
                             WHERE email_address = @Email 
                             AND password = @Password 
                             AND role = @Role";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Role", role);

                        int count = (int)cmd.ExecuteScalar(); // Get number of matching rows

                        if (count > 0)
                        {
                            isfound = true;
                            Console.WriteLine("User found");
                        }
                        else
                        {
                            Console.WriteLine("User not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return isfound;
        }

        public void Create()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    string sql_query = @"
                CREATE TABLE Claims (
                    Claim_Id INT IDENTITY(200,1) PRIMARY KEY,
                    EmployeeNum INT FOREIGN KEY REFERENCES users(userID),
                    Lecture_Name VARCHAR(50) NOT NULL,
                    Module_Name VARCHAR(50) NOT NULL,
                    Number_ofSessions INT NOT NULL,
                    Hourly_rate DECIMAL(10,2) NOT NULL,
                    Total_Amount DECIMAL(10,2) NOT NULL,
                    Claim_Start_Date DATE NOT NULL,
                    Claim_End_Date DATE NOT NULL,
                    Claim_Description VARCHAR(255),
                    Document_Data VARBINARY(MAX),
                    Claim_Status VARCHAR(20) DEFAULT 'Pending'
                );";

                    using (SqlCommand command = new SqlCommand(sql_query, connect))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("✅ Claims table created successfully!");
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("❌ Error creating Claims table: " + error.Message);
            }
        }

        public bool UserExists(int employeeNum)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                string query = "SELECT COUNT(*) FROM users WHERE userID = @EmployeeNum";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@EmployeeNum", employeeNum);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void StoreClaimTable(
            int EmployeeNum,
            string LectureName,
            string ModuleName,
            int NumberOfSessions,
            double HourlyRate,
            DateTime ClaimStartDate,
            DateTime ClaimEndDate,
            string ClaimDescription,
            byte[] DocumentData)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    // Calculate total amount
                    decimal TotalAmount = (decimal)(NumberOfSessions * HourlyRate);

                    string sql_query = @"
            INSERT INTO Claims 
            (EmployeeNum, Lecture_Name, Module_Name, Number_ofSessions, Hourly_rate, Total_Amount, 
             Claim_Start_Date, Claim_End_Date, Claim_Description, Document_Data, Claim_Status)
            VALUES 
            (@EmployeeNum, @LectureName, @ModuleName, @NumberOfSessions, @HourlyRate, @TotalAmount, 
             @ClaimStartDate, @ClaimEndDate, @ClaimDescription, @DocumentData, @ClaimStatus)";

                    using (SqlCommand command = new SqlCommand(sql_query, connect))
                    {
                        command.Parameters.AddWithValue("@EmployeeNum", EmployeeNum);
                        command.Parameters.AddWithValue("@LectureName", LectureName);
                        command.Parameters.AddWithValue("@ModuleName", ModuleName);
                        command.Parameters.AddWithValue("@NumberOfSessions", NumberOfSessions);
                        command.Parameters.AddWithValue("@HourlyRate", TotalAmount / NumberOfSessions); // exact decimal
                        command.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                        command.Parameters.AddWithValue("@ClaimStartDate", ClaimStartDate);
                        command.Parameters.AddWithValue("@ClaimEndDate", ClaimEndDate);
                        command.Parameters.AddWithValue("@ClaimDescription", ClaimDescription ?? "");
                        command.Parameters.AddWithValue("@DocumentData", DocumentData ?? new byte[0]);
                        command.Parameters.AddWithValue("@ClaimStatus", "Pending");

                        command.ExecuteNonQuery();
                        Console.WriteLine("✅ Claim stored successfully!");
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("❌ Error while storing claim: " + error.Message);
                throw; // re-throw to see errors in VS output
            }
        }

        public List<Claim> GetClaimsByEmployee(int employeeNum)
        {
            var claims = new List<Claim>();

            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = @"SELECT Claim_Id, EmployeeNum, Lecture_Name, Module_Name, Number_ofSessions, 
                                    Hourly_rate, Total_Amount, Claim_Start_Date, Claim_End_Date, 
                                    Claim_Description, Claim_Status
                             FROM Claims
                             WHERE EmployeeNum = @EmployeeNum
                             ORDER BY Claim_Id DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeNum", employeeNum);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                claims.Add(new Claim
                                {
                                    Claim_Id = reader.GetInt32(0),
                                    EmployeeNum = reader.GetInt32(1),
                                    Lecture_Name = reader.GetString(2),
                                    Module_Name = reader.GetString(3),
                                    Number_ofSessions = reader.GetInt32(4),
                                    Hourly_rate = reader.GetDecimal(5),
                                    Total_Amount = reader.GetDecimal(6),
                                    Claim_Start_Date = reader.GetDateTime(7),
                                    Claim_End_Date = reader.GetDateTime(8),
                                    Claim_Description = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    Claim_Status = reader.IsDBNull(10) ? "Pending" : reader.GetString(10)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error fetching claims: " + ex.Message);
            }

            return claims;
        }

        public List<Claim> GetAllClaims()
        {
            var claims = new List<Claim>();

            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = @"SELECT Claim_Id, EmployeeNum, Lecture_Name, Module_Name, Number_ofSessions, 
                                    Hourly_rate, Total_Amount, Claim_Start_Date, Claim_End_Date, 
                                    Claim_Description, Claim_Status
                             FROM Claims
                             ORDER BY Claim_Id DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            claims.Add(new Claim
                            {
                                Claim_Id = reader.GetInt32(0),
                                EmployeeNum = reader.GetInt32(1),
                                Lecture_Name = reader.GetString(2),
                                Module_Name = reader.GetString(3),
                                Number_ofSessions = reader.GetInt32(4),
                                Hourly_rate = reader.GetDecimal(5),
                                Total_Amount = reader.GetDecimal(6),
                                Claim_Start_Date = reader.GetDateTime(7),
                                Claim_End_Date = reader.GetDateTime(8),
                                Claim_Description = reader.IsDBNull(9) ? null : reader.GetString(9),
                                Claim_Status = reader.IsDBNull(10) ? "Pending" : reader.GetString(10)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error fetching all claims: " + ex.Message);
            }

            return claims;
        }

        public void UpdateClaimStatus(int claimId, string newStatus)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = "UPDATE Claims SET Claim_Status = @Status WHERE Claim_Id = @ClaimId";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@ClaimId", claimId);
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"✅ Claim #{claimId} updated to {newStatus}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error updating claim: " + ex.Message);
            }
        }

        public void UpdateClaimStatusByManager(int claimId, string newStatus)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();
                    string query = "UPDATE Claims SET Claim_Status = @Status WHERE Claim_Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@Id", claimId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error updating claim status by manager: " + ex.Message);
            }
        }

    }
}
