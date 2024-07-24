using RegisterAndLoginApp.Models;
using System.Data.SqlClient;
/*
 * Dakoda Meade
 * CST-350
 * Activity 2-2
 * SecurityDAO class that checks if a usere existes in the databse
 */
namespace RegisterAndLoginApp.Services
{
    public class SecurityDAO
    {
        // connection string for our database
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;
        TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// This method is for looking up users in the database
        /// takes a usermodel object as a parameter
        /// </summary>
        /// <param name="user"></param>
        /// <returns>if a user is valid or not</returns>
        public bool FindByNameAndPassword(UserModel user)
        {
            //assume nothing is found
            bool success = false;

            // uses prpared statements for security. @username and @ password are defined below 
            string sqlStatement = "SELECT * FROM dbo.USERS WHERE username = @username and password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command  = new SqlCommand(sqlStatement, connection);

                //define the values of the two placeholers in the sqlStatment string

                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 50).Value = user.username;
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50).Value = user.password;

                try 
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if(reader.HasRows) 
                    {
                        //user is valid else flase
                        success = true;
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                };
            }
            // return
            return success;
        }
    }
}
