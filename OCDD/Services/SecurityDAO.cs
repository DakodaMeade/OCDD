using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using OCDD.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
/*
 * Dakoda Meade
 * SecurityDAO classworking with user information in database
 * Handles user informatino interactions with the databse
 */
namespace OCDD.Services
{
	public class SecurityDAO
	{
		
		// connection string for our database
		//string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OCDDetailing_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;
        //TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //local db
        //string connectionString = "Server=127.0.0.1;Database=ocddetailing_db;User ID=root;Password=root;Pooling=false;";

        // Azure conntection string
        string connectionString = "Server=ocddetailingmysql.mysql.database.azure.com;Database=ocddetailing_db;User ID=dmeade;Password=Cpt.Cuddles96;Pooling=false;";

        /// <summary>
        /// Finsds the user by the email and password
        /// </summary>
        /// <param name="user"> user object </param>
        /// <returns> Returns if user is found or not  </returns>
        public bool FindByEmailAndPassword(UserModel user)
        {
            // initialize to false by default
            bool success = false;
            string sqlStatement = "SELECT password FROM Users WHERE email = @email";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {

                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                    
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Connection to database is Not successful!");
            }
            // returns if success or failure
            return success;
        }

        /// <summary>
        /// returns a complete UserModel based on a user's Id
        /// </summary>
        /// <param name="userId"> user id </param>
        /// <returns> The user </returns>
        public UserModel GetUserById(int userId)
        {
            UserModel resultUser = null;

            //string sqlStatement = "SELECT * FROM Users WHERE userID = @userId";
            // Updated SQL statement with JOIN
            string sqlStatement = @"
            SELECT u.*, r.name AS roleName
            FROM Users u
            JOIN Roles r ON u.roleID = r.roleID
            WHERE u.userID = @userId";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                    command.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;


                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // read the row
                        reader.Read(); 

                        
                        int id = reader.GetInt32(reader.GetOrdinal("userID"));
                        // user properties 
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string phoneNumber = reader.GetString(reader.GetOrdinal("phoneNumber"));
                        string address = reader.GetString(reader.GetOrdinal("address"));
                        int zipCode = reader.GetInt32(reader.GetOrdinal("zipCode"));
                        string city = reader.GetString(reader.GetOrdinal("city"));
                        string state = reader.GetString(reader.GetOrdinal("state"));
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string roleName = reader.GetString(reader.GetOrdinal("roleName"));
                        // store result into user object
                        resultUser = new UserModel
                        {
                            userID = userId,
                            name = name,
                            phoneNumber = phoneNumber,
                            address = address,
                            zipCode = zipCode,
                            state = state,
                            email = email,
                            city = city,
                            role = roleName,
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // return the found user object
            return resultUser;
        }

        /// <summary>
        /// Method to add a new user to the database
        /// </summary>
        /// <param name="user"> user object </param>
        public void AddUser(UserModel user)
        {
            // insert staement for user object
            string sqlStatement = "INSERT INTO Users (name, phoneNumber, address, zipCode, city, state, email, password) VALUES (@name, @phoneNumber, @address, @zipCode, @city, @state, @email, @password)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// Checks if the emails exists in the database
        /// </summary>
        /// <param name="user"> user object </param>
        /// <returns>true if email exists false if not</returns>
        public bool FindUserByEmail(UserModel user)
        {
            // set exists = false 
            bool exists = false;
            // select if user name or email matches
            string sqlStatement = "SELECT * FROM Users WHERE email = @email";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);


                    command.Parameters.Add("@email", MySqlDbType.VarChar, 40).Value = user.email;

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    // a row grabbed from the query so that means a user with that exits so we set to true
                    if (reader.HasRows)
                    {
                        exists = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // if email exists or not
            return exists;
        }

        /// <summary>
        /// Obtains user id with the email
        /// </summary>
        /// <param name="user"> user object</param>
        /// <returns> the user id </returns>
        public int FindUserIDByEmail(UserModel user)
        {
            // userID to 0
            int userID = 0;
            // select if user name or email matches
            string sqlStatement = "SELECT * FROM Users WHERE email = @email";


            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);


                    command.Parameters.Add("@email", MySqlDbType.VarChar, 40).Value = user.email;

                    
                    
                     connection.Open();
                     MySqlDataReader reader = command.ExecuteReader();
                     // a row grabbed from the query so that means a user with that exits so we set to true
                     if (reader.HasRows)
                     {
                         reader.Read();
                         userID = reader.GetInt32(reader.GetOrdinal("userID"));
                     }
                    
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // returns the user id
            return userID;
        }

        /// <summary>
        /// Method for updating the user's profile not including the password
        /// </summary>
        /// <param name="user">user object </param>
        public void UpdateProfile(UserModel user)
        {
            // update statement
            string sqlStatement = "UPDATE users SET name = @name, phoneNumber = @phoneNumber, city = @city, address = @address, zipCode = @zipCode, state = @state, email = @email WHERE userID = @userID";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // set the user inforamtion 
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@userID", user.userID);
                    command.Parameters.AddWithValue("@name", user.name);
                    command.Parameters.AddWithValue("@phoneNumber", user.phoneNumber);
                    command.Parameters.AddWithValue("@address", user.address);
                    command.Parameters.AddWithValue("@zipCode", user.zipCode);
                    command.Parameters.AddWithValue("@city", user.city);
                    command.Parameters.AddWithValue("@state", user.state);
                    command.Parameters.AddWithValue("@email", user.email);


                    
                    connection.Open();
                    command.ExecuteNonQuery();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Update the user password in the database
        /// </summary>
        /// <param name="user">user object</param>
        public void UpdatePassword(UserModel user)
        {
            // update statement
            string sqlStatement = "UPDATE users SET password = @password WHERE userID = @userID";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@userID", user.userID);

                    // Hash the password before storing in the database
                    string hashedPassword = HashPassword(user.password);
                    command.Parameters.AddWithValue("@password", hashedPassword);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Hashes the password using SHA 256 for database storage
        /// </summary>
        /// <param name="password"> password </param>
        /// <returns>the hashed password</returns>
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
