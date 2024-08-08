using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using OCDD.Models;
using System.Web.Helpers;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OCDD.Services
{
    public class AppointmentDAO
    {
        // database connection string
        string connectionString = "Server=127.0.0.1;Database=ocddetailing_db;User ID=root;Password=root;Pooling=false;";


        // Save the appointment to the database
        public int SaveAppointmentUser(AppointmentModel appointment)
        {
            int appointmentId = 0;
            try
            {
                
                string sqlStatement = "INSERT INTO appointments (userID, serviceID, dateTime, vehicleYear, vehicleMake, vehicleModel) VALUES (@userID, @serviceID, @dateTime, @vehicleYear, @vehicleMake, @vehicleModel); SELECT LAST_INSERT_ID();";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@UserID", appointment.user.userID);
                    command.Parameters.AddWithValue("@ServiceID", appointment.service.serviceID);
                    command.Parameters.AddWithValue("@dateTime", appointment.dateTime);
                    command.Parameters.AddWithValue("@vehicleYear", appointment.vehicleYear);
                    command.Parameters.AddWithValue("@vehicleMake", appointment.vehicleMake);
                    command.Parameters.AddWithValue("@vehicleModel", appointment.vehicleModel);

                    connection.Open();
                    appointmentId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return appointmentId;
        }

        // Save the appointment to the database
        public int SaveAppointmentNonUser(AppointmentModel appointment)
        {
            int appointmentId = 0;

            try
            {

                string sqlStatement = "INSERT INTO appointments (serviceID, dateTime, vehicleYear, vehicleMake, vehicleModel, name, phoneNumber, address, zipCode, city, state, email) VALUES (@serviceID, @dateTime, @vehicleYear, @vehicleMake, @vehicleModel, @name, @phoneNumber, @address, @zipCode, @city, @state, @email); SELECT LAST_INSERT_ID();";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                    command.Parameters.AddWithValue("@serviceID", appointment.service.serviceID);
                    command.Parameters.AddWithValue("@dateTime", appointment.dateTime);
                    command.Parameters.AddWithValue("@vehicleYear", appointment.vehicleYear);
                    command.Parameters.AddWithValue("@vehicleMake", appointment.vehicleMake);
                    command.Parameters.AddWithValue("@vehicleModel", appointment.vehicleModel);
                    command.Parameters.AddWithValue("@name", appointment.name);
                    command.Parameters.AddWithValue("@phoneNumber", appointment.phoneNumber);
                    command.Parameters.AddWithValue("@address", appointment.address);
                    command.Parameters.AddWithValue("@zipCode", appointment.zipCode);
                    command.Parameters.AddWithValue("@city", appointment.city);
                    command.Parameters.AddWithValue("@state", appointment.state);
                    command.Parameters.AddWithValue("@email", appointment.email);

                    connection.Open();
                    appointmentId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointmentId;
        }

        // Return the appointment based on the ID
        public AppointmentModel GetAppointmentByID(int appointmentID)
        {

            AppointmentModel appointment = null;
            bool userAppointment = IsUserAppointment(appointmentID); // return if a logged in user created
            try
            {
                
                string sqlStatement;
                // this returns non user appoitnments or with corrent details even if the user registered

                if (!userAppointment)
                {
                    sqlStatement = @"
            SELECT 
                a.serviceID, a.dateTime, a.vehicleYear, a.vehicleMake, a.vehicleModel, a.name, a.phoneNumber, a.address, a.zipCode, a.city, a.state, a.email,
                s.name AS serviceName, s.description, s.price, s.duration, st.status
            FROM 
                appointments a
            JOIN 
                services s ON a.serviceID = s.serviceID
            JOIN
                appointment_status st ON a.statusID = st.statusID
            WHERE 
                a.appointmentID = @appointmentID";
                }
                else
                {
                    sqlStatement = @"
            SELECT 
                a.serviceID, a.dateTime, a.vehicleYear, a.vehicleMake, a.vehicleModel,
                s.name AS serviceName, s.description, s.price, s.duration,
                u.userID, u.name, u.phoneNumber, u.address, u.zipCode, u.city, u.state, u.email,
                st.status
            FROM 
                appointments a
            JOIN 
                services s ON a.serviceID = s.serviceID
            JOIN
                users u ON a.userID = u.userID
            JOIN
                appointment_status st ON a.statusID = st.statusID
            WHERE 
                a.appointmentID = @appointmentID";
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@appointmentID", appointmentID);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            appointment = new AppointmentModel
                            {
                                appointmentID = appointmentID,
                                service = new ServiceModel
                                {
                                    serviceID = reader.GetInt32("serviceID"),
                                    name = reader.GetString("serviceName"),
                                    description = reader.GetString("description"),
                                    price = reader.GetDecimal("price"),
                                    duration = reader.GetTimeSpan("duration")
                                },
                                dateTime = reader.GetDateTime("dateTime"),
                                date = reader.GetDateTime("dateTime").Date,
                                time = reader.GetDateTime("dateTime").TimeOfDay,
                                vehicleYear = reader.GetInt32("vehicleYear"),
                                vehicleMake = reader.GetString("vehicleMake"),
                                vehicleModel = reader.GetString("vehicleModel"),
                                status = reader.GetString("status"),
                                name = reader.GetString("name"),
                                phoneNumber = reader.GetString("phoneNumber"),
                                address = reader.GetString("address"),
                                zipCode = reader.GetInt32("zipCode"),
                                city = reader.GetString("city"),
                                state = reader.GetString("state"),
                                email = reader.GetString("email")
                            };
                            
                            if (userAppointment)
                            {
                                appointment.user = new UserModel
                                {
                                    userID = reader.GetInt32("userID"),
                                    name = reader.GetString("name"),
                                    phoneNumber = reader.GetString("phoneNumber"),
                                    address = reader.GetString("address"),
                                    zipCode = reader.GetInt32("zipCode"),
                                    city = reader.GetString("city"),
                                    state = reader.GetString("state"),
                                    email = reader.GetString("email")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointment;
        }
       



        public List<AppointmentModel> GetAppointmentsByUserID(int userID)
        {
            List<AppointmentModel> appointments = new List<AppointmentModel>();
            bool userAppointment = false;

            string sqlStatement = @"
        SELECT 
            a.appointmentID, a.serviceID, a.dateTime, a.vehicleYear, a.vehicleMake, a.vehicleModel, a.name, a.phoneNumber, a.address, a.zipCode, a.city, a.state, a.email,
            s.name AS serviceName, s.description, s.price, s.duration,
            u.name AS userName, u.phoneNumber AS userPhoneNumber, u.address AS userAddress, u.zipCode AS userZipCode, u.city AS userCity, u.state AS userState, u.email AS userEmail,
            st.status
        FROM 
            appointments a
        JOIN 
            services s ON a.serviceID = s.serviceID
        JOIN
            users u ON a.userID = u.userID
        JOIN
            appointment_status st ON a.statusID = st.statusID
        WHERE 
            a.userID = @userID";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@userID", userID);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var appointment = new AppointmentModel
                            {
                                appointmentID = reader.GetInt32("appointmentID"),
                                service = new ServiceModel
                                {
                                    serviceID = reader.GetInt32("serviceID"),
                                    name = reader.GetString("serviceName"),
                                    description = reader.GetString("description"),
                                    price = reader.GetDecimal("price"),
                                    duration = reader.GetTimeSpan("duration")
                                },
                                dateTime = reader.GetDateTime("dateTime"),
                                date = reader.GetDateTime("dateTime").Date,
                                time = reader.GetDateTime("dateTime").TimeOfDay,
                                vehicleYear = reader.GetInt32("vehicleYear"),
                                vehicleMake = reader.GetString("vehicleMake"),
                                vehicleModel = reader.GetString("vehicleModel"),
                                status = reader.GetString("status"),

                            };
                            userAppointment = IsUserAppointment(appointment.appointmentID);
                            if (!userAppointment)
                            {
                                appointment.name = reader.GetString("name");
                                appointment.phoneNumber = reader.GetString("phoneNumber");
                                appointment.address = reader.GetString("address");
                                appointment.zipCode = reader.GetInt32("zipCode");
                                appointment.city = reader.GetString("city");
                                appointment.state = reader.GetString("state");
                                appointment.email = reader.GetString("email");
                            }
                            else 
                            {
                                appointment.name = reader.GetString("userName");
                                appointment.phoneNumber = reader.GetString("userPhoneNumber");
                                appointment.address = reader.GetString("userAddress");
                                appointment.zipCode = reader.GetInt32("userZipCode");
                                appointment.city = reader.GetString("userCity");
                                appointment.state = reader.GetString("userState");
                                appointment.email = reader.GetString("userEmail");
                            }
                            appointments.Add(appointment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;
        }

        public void UpdateAppointmentsWithUserID(string email, int userID)
        {
            string sqlStatement = @"
        UPDATE appointments
        SET userID = @UserID
        WHERE email = @Email";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CompleteAppointment(int appointmentID) 
        {
            string sqlStatement = @"
        UPDATE appointments
        SET statusID = 3
        WHERE appointmentID = @appointmentID";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@appointmentID", appointmentID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CancelAppointment(int appointmentID)
        {
            string sqlStatement = @"
        UPDATE appointments
        SET statusID = 2
        WHERE appointmentID = @appointmentID";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@appointmentID", appointmentID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // returns if the name in the appointments table is null if the name column is null it returns true which means it is a user scheduled appoitnment 
        // this is so that if a user registers after all creating an appoitnemnts he can see the details he used from when registered and not user details
        public bool IsUserAppointment(int appointmentID)
        {
            bool isNull = false;
            try
            {

                string sqlStatement = "SELECT name FROM appointments WHERE appointmentID = @appointmentID";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@appointmentID", appointmentID);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isNull = reader.IsDBNull(reader.GetOrdinal("name"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return isNull;
        }

        // get all the appoitnemtns for the admin
        public List<AppointmentModel> GetAllAppointments()
        {
            List<AppointmentModel> appointments = new List<AppointmentModel>();
            bool userAppointment = false;

            string sqlStatement = @"
        SELECT
            a.appointmentID, a.serviceID, a.dateTime, a.vehicleYear, a.vehicleMake, a.vehicleModel, a.name, a.phoneNumber, a.address, a.zipCode, a.city, a.state, a.email,
            s.name AS serviceName, s.description, s.price, s.duration,
            u.name AS userName, u.phoneNumber AS userPhoneNumber, u.address AS userAddress, u.zipCode AS userZipCode, u.city AS userCity, u.state AS userState, u.email AS userEmail,
            st.status
        FROM 
            appointments a
        JOIN 
            services s ON a.serviceID = s.serviceID
        JOIN
            users u ON a.userID = u.userID
        JOIN
            appointment_status st ON a.statusID = st.statusID
        ORDER BY
            a.dateTime DESC
        LIMIT 2000;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);


                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var appointment = new AppointmentModel
                            {
                                appointmentID = reader.GetInt32("appointmentID"),
                                service = new ServiceModel
                                {
                                    serviceID = reader.GetInt32("serviceID"),
                                    name = reader.GetString("serviceName"),
                                    description = reader.GetString("description"),
                                    price = reader.GetDecimal("price"),
                                    duration = reader.GetTimeSpan("duration")
                                },
                                dateTime = reader.GetDateTime("dateTime"),
                                date = reader.GetDateTime("dateTime").Date,
                                time = reader.GetDateTime("dateTime").TimeOfDay,
                                vehicleYear = reader.GetInt32("vehicleYear"),
                                vehicleMake = reader.GetString("vehicleMake"),
                                vehicleModel = reader.GetString("vehicleModel"),
                                status = reader.GetString("status"),

                            };
                            userAppointment = IsUserAppointment(appointment.appointmentID);
                            if (!userAppointment)
                            {
                                appointment.name = reader.GetString("name");
                                appointment.phoneNumber = reader.GetString("phoneNumber");
                                appointment.address = reader.GetString("address");
                                appointment.zipCode = reader.GetInt32("zipCode");
                                appointment.city = reader.GetString("city");
                                appointment.state = reader.GetString("state");
                                appointment.email = reader.GetString("email");
                            }
                            else
                            {
                                appointment.name = reader.GetString("userName");
                                appointment.phoneNumber = reader.GetString("userPhoneNumber");
                                appointment.address = reader.GetString("userAddress");
                                appointment.zipCode = reader.GetInt32("userZipCode");
                                appointment.city = reader.GetString("userCity");
                                appointment.state = reader.GetString("userState");
                                appointment.email = reader.GetString("userEmail");
                            }
                            appointments.Add(appointment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;
        }


        public List<DateTime> GetAvailableTimeSlots(DateTime date)
        {
            List<DateTime> availableSlots = new List<DateTime>();
            List<TimeSlot> bookedSlots = new List<TimeSlot>();

            try
            {
                // Step 1: Retrieve all booked slots and their durations
                string sqlStatement = @"
            SELECT a.dateTime AS startTime, 
                   DATE_ADD(a.dateTime, INTERVAL s.duration MINUTE) AS endTime
            FROM appointments a
            JOIN services s ON a.serviceID = s.serviceID
            WHERE DATE(a.dateTime) = @date
            ORDER BY startTime";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime startTime = reader.GetDateTime("startTime");
                        DateTime endTime = reader.GetDateTime("endTime");

                        bookedSlots.Add(new TimeSlot { StartTime = startTime, EndTime = endTime });
                    }
                }

                // Step 2: Define working hours
                TimeSpan workingStartTime = new TimeSpan(8, 0, 0); // 8 AM
                TimeSpan workingEndTime = new TimeSpan(17, 0, 0);  // 5 PM

                // Step 3: Check for available slots between booked appointments
                DateTime currentSlotStart = date.Date.Add(workingStartTime);

                foreach (var bookedSlot in bookedSlots)
                {
                    // Check for available slot before the current booked slot
                    if (currentSlotStart < bookedSlot.StartTime)
                    {
                        availableSlots.Add(currentSlotStart);
                    }
                    currentSlotStart = bookedSlot.EndTime;
                }

                // Check for available slot after the last booked slot
                if (currentSlotStart < date.Date.Add(workingEndTime))
                {
                    availableSlots.Add(currentSlotStart);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return availableSlots;
        }

        public class TimeSlot
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }


    }
}
