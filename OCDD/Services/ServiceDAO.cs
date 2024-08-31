/*
 * Dakoda Meade
 * ServiceDAO classworking with service information in database
 */
using MySql.Data.MySqlClient;
using OCDD.Models;

namespace OCDD.Services
{
    public class ServiceDAO
    {
        // connection string for database local
       // string connectionString = "Server=127.0.0.1;Database=ocddetailing_db;User ID=root;Password=root;Pooling=false;";

        //Azure database connection string
        string connectionString = "Server=ocddetailingmysql.mysql.database.azure.com;Database=ocddetailing_db;User ID=dmeade;Password=Cpt.Cuddles96;Pooling=false;";
        /// <summary>
        /// get all services from database
        /// </summary>
        /// <returns> All the servies in the data base</returns>
        public List<ServiceModel> GetServices()
        {
            // inistialize service model list
            List<ServiceModel> services = new List<ServiceModel>();
            try
            {
                // sql statement to get non deleted services
                string sqlStatement = "SELECT * FROM services WHERE isDeleted = 0";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // create service model object from rows of the database
                        ServiceModel service = new ServiceModel
                        {
                            serviceID = reader.GetInt32("serviceID"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description"),
                            duration = reader.GetTimeSpan("duration"),
                            price = reader.GetDecimal("price")
                        };
                        // add the boject to the list
                        services.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // service list is returned
            return services;
        }

        /// <summary>
        /// Get service with the service ID number
        /// </summary>
        /// <param name="serviceID">Slected service id</param>
        /// <returns> service object found in database </returns>
        public ServiceModel GetServiceByID(int serviceID)
        {
            // initialize the serice object as null 
            ServiceModel service = null;
            try
            {
                // sql select staement to find the service based on the id that is not deleted
                string sqlStatement = "SELECT * FROM services WHERE isDeleted = 0 AND serviceID = @serviceID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@serviceID", serviceID);

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // create the service object
                        service = new ServiceModel
                        {
                            serviceID = reader.GetInt32("serviceID"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description"),
                            duration = reader.GetTimeSpan("duration"),
                            price = reader.GetDecimal("price")
                        };
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // return the service object
            return service;
        }

        /// <summary>
        /// saves the service but if the service id = 0 this it needs to be added and not updated
        /// </summary>
        /// <param name="service">service object</param>
        public void SaveService(ServiceModel service)
        {
            string sqlStatement;
            try
            {
                // If the service is new use insert statement
                // if the service is not new use update statement
                if (service.serviceID == 0)
                {
                    sqlStatement = "INSERT INTO services (name, description, price, duration ) VALUES (@name, @description, @price, @duration);";
                }
                else 
                {
                    sqlStatement = "UPDATE services SET name = @name, description = @description, price = @price, duration = @duration WHERE serviceID = @serviceID;";
                }
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@name", service.name);
                    command.Parameters.AddWithValue("@description", service.description);
                    command.Parameters.AddWithValue("@price", service.price);
                    command.Parameters.AddWithValue("@duration", service.duration);
                    if (service.serviceID != 0)
                    {
                        command.Parameters.AddWithValue("@serviceID", service.serviceID);
                    }

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
        /// Delete service. Doesnot actually remove it from the table but changes the column isDetlete to 1. so that it seems like it is deleted. that way we can still see it on old appoitnemnts
        /// </summary>
        /// <param name="serviceID"> selected service id </param>
        public void DeleteService(int serviceID)
        {
            // sql update statement to update the service
            string sqlStatement = "UPDATE services SET isDeleted = 1 WHERE serviceID = @serviceID;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                   
                    command.Parameters.AddWithValue("@serviceID", serviceID);
                    

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
