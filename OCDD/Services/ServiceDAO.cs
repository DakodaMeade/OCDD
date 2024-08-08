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
        // connection string for database
        string connectionString = "Server=127.0.0.1;Database=ocddetailing_db;User ID=root;Password=root;Pooling=false;";
        // get all services from database
        public List<ServiceModel> GetServices()
        {

            List<ServiceModel> services = new List<ServiceModel>();
            try
            {
                string sqlStatement = "SELECT * FROM services WHERE isDeleted = 0";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ServiceModel service = new ServiceModel
                        {
                            serviceID = reader.GetInt32("serviceID"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description"),
                            duration = reader.GetTimeSpan("duration"),
                            price = reader.GetDecimal("price")
                        };

                        services.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return services;
        }

        // Get service with the service ID number
        public ServiceModel GetServiceByID(int serviceID)
        {

            ServiceModel service = null;
            try
            {
                string sqlStatement = "SELECT * FROM services WHERE isDeleted = 0 AND serviceID = @serviceID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@serviceID", serviceID);

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
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

            return service;
        }

        // saves the service but if the appoitnemnt id = 0 this it needs to be added and not updated
        public void SaveService(ServiceModel service)
        {
            string sqlStatement;
            try
            {
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



        // Delete service. Doesnot actually remove it from the table but changes the column isDetlete to 1. so that it seems like it is deleted. that way we can still see it on old appoitnemnts
        public void DeleteService(int serviceID)
        {
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
