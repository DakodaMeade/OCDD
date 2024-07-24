using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using OCDD.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
/*
 * Dakoda Meade
 * CST-350
 * Activity 2-2
 * SecurityDAO class that checks if a usere existes in the databse
 */
namespace OCDD.Services
{
	public class SecurityDAO
	{
		
		// connection string for our database
		//string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OCDDetailing_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;
        //TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        string connectionString = "Server=127.0.0.1;Database=ocddetailing_db;User ID=root;Password=root;Pooling=false;";



        /// <summary>
        /// This method is for looking up users in the database
        /// takes a usermodel object as a parameter
        /// </summary>
        /// <param name="user"></param>
        /// <returns>if a user is valid or not</returns>
        /// 
        /*public bool FindByEmailAndPassword(UserModel user)
		{
			//assume nothing is found
			bool success = false;

			// uses prpared statements for security. @username and @ password are defined below 
			string sqlStatement = "SELECT * FROM dbo.USERS WHERE username = @username and password = @password";
			// used to store the password from he database
            string storedHashedPassword = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				

				try
				{	
					MySqlCommand command = new MySqlCommand(sqlStatement, connection);

					//define the values of the two placeholers in the sqlStatment string

					command.Parameters.Add("@email", (MySqlDbType)System.Data.SqlDbType.VarChar, 50).Value = user.email;
					command.Parameters.Add("@password", (MySqlDbType)System.Data.SqlDbType.VarChar, 50).Value = user.password;
					connection.Open();
                    Console.WriteLine("Connection to database is successful!");
                    MySqlDataReader reader = command.ExecuteReader();

					if (reader.HasRows)
					{
						//user is valid else false
						storedHashedPassword = reader["password"] as string;
						
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
                    Console.WriteLine("Connection to database is Not successful!");
                };

                if (storedHashedPassword != null)
                {
                    // Hash the provided password and compare it with the stored hash
                    string hashedPassword = HashPassword(user.password);
                    return hashedPassword == storedHashedPassword;
                }


            }
			// return
			return success;
		}*/
        public bool FindByEmailAndPassword(UserModel user)
        {
            bool success = false;
            string sqlStatement = "SELECT password FROM Users WHERE email = @email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                    // Use AddWithValue for MySQL parameters
                    command.Parameters.AddWithValue("@email", user.email);

                    connection.Open();
                    Console.WriteLine("Connection to database is successful!");

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Retrieve the hashed password from the database
                        string storedHashedPassword = reader["password"] as string;

                        if (storedHashedPassword != null)
                        {
                            // Hash the provided password and compare it with the stored hash
                            string hashedPassword = HashPassword(user.password);
                            success = hashedPassword == storedHashedPassword;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Connection to database is Not successful!");
                }
            }

            return success;
        }
        /*
		// Method to add a new user to the database
		public bool AddUser(UserModel user)
		{
			bool success = false;

			string sqlStatement = "INSERT INTO dbo.Users (name, phoneNumber, address, zipCode, city, state, email, password) values (@name, @phoneNumber, @address, @zipCode, @city, @state, @email, @password)";

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				try
				{
					MySqlCommand command = new MySqlCommand(sqlStatement, connection);

					command.Parameters.Add("@Name", (MySqlDbType)System.Data.SqlDbType.VarChar, 20).Value = user.name;
					command.Parameters.Add("@PhoneNumber", (MySqlDbType)System.Data.SqlDbType.Int).Value = user.phoneNumber;
					command.Parameters.Add("@Address", (MySqlDbType)System.Data.SqlDbType.VarChar, 20).Value = user.address;
					command.Parameters.Add("@ZipCode", (MySqlDbType)System.Data.SqlDbType.Int).Value = user.zipCode;
                    command.Parameters.Add("@City", (MySqlDbType)System.Data.SqlDbType.VarChar, 20).Value = user.city;
                    command.Parameters.Add("@State", (MySqlDbType)System.Data.SqlDbType.VarChar, 2).Value = user.state;
					command.Parameters.Add("@Email", (MySqlDbType)System.Data.SqlDbType.VarChar, 40).Value = user.email;

					// Hash password for more security before storing in the database
					string hashedPassword = HashPassword(user.password);

					command.Parameters.Add("@password", (MySqlDbType)System.Data.SqlDbType.VarChar, 64).Value = hashedPassword;

					connection.Open();
					//SqlDataReader reader = command.ExecuteReader();
					command.ExecuteNonQuery();
					success = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return success;

		}*/

        // Method to add a new user to the database
        public void AddUser(UserModel user)
        {
            
            string sqlStatement = "INSERT INTO Users (name, phoneNumber, address, zipCode, city, state, email, password) VALUES (@name, @phoneNumber, @address, @zipCode, @city, @state, @email, @password)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@name", user.name);
                    command.Parameters.AddWithValue("@phoneNumber", user.phoneNumber);
                    command.Parameters.AddWithValue("@address", user.address);
                    command.Parameters.AddWithValue("@zipCode", user.zipCode);
                    command.Parameters.AddWithValue("@city", user.city);
                    command.Parameters.AddWithValue("@state", user.state);
                    command.Parameters.AddWithValue("@email", user.email);

                    // Hash the password before storing in the database
                    string hashedPassword = HashPassword(user.password);
                    command.Parameters.AddWithValue("@password", hashedPassword);

                    connection.Open();
                    command.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

          
        }

        public bool FindUserByEmail(UserModel user)
        {
            // set exists = false 
            bool exists = false;
            // select if user name or email matches
            string sqlStatement = "SELECT * FROM dbo.Users WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

               
                command.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 40).Value = user.email;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    // a row grabbed from the query so that means a user with that exits so we set to true
                    if (reader.HasRows)
                    {
                        exists = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return exists;
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
