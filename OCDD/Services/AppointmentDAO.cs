using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using OCDD.Models;
using System.Web.Helpers;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
/*
 * Dakoda Meade
 * Appointment DAO Class
 * Handles all interactions with appoitnemtn information with the database
 */
namespace OCDD.Services
{
    public class AppointmentDAO
    {
        // database connection string Local
        //string connectionString = "Server=127.0.0.1;Database=ocddetailing_db;User ID=root;Password=root;Pooling=false;";
        // Azure conntection string
        string connectionString = "Server=ocddetailingmysql.mysql.database.azure.com;Database=ocddetailing_db;User ID=dmeade;Password=Cpt.Cuddles96;Pooling=false;";
        /// <summary>
        /// Save the appointment to the database for a user
        /// </summary>
        /// <param name="appointment">Appointment object</param>
        /// <returns>The appoinmtentID of the appoitnemt what was just entered into the databse</returns>
        public int SaveAppointmentUser(AppointmentModel appointment)
        {
            int appointmentId = 0;
            try
            {
                // MY SQL statement
                string sqlStatement = "INSERT INTO appointments (userID, serviceID, dateTime, vehicleYear, vehicleMake, vehicleModel) VALUES (@userID, @serviceID, @dateTime, @vehicleYear, @vehicleMake, @vehicleModel); SELECT LAST_INSERT_ID();";// selected the last entered id to return
                 
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // set up command
                    MySqlCommand command = new MySqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@UserID", appointment.user.userID);
                    command.Parameters.AddWithValue("@ServiceID", appointment.service.serviceID);
                    command.Parameters.AddWithValue("@dateTime", appointment.dateTime);
                    command.Parameters.AddWithValue("@vehicleYear", appointment.vehicleYear);
                    command.Parameters.AddWithValue("@vehicleMake", appointment.vehicleMake);
                    command.Parameters.AddWithValue("@vehicleModel", appointment.vehicleModel);

                    connection.Open();
                    appointmentId = Convert.ToInt32(command.ExecuteScalar()); // store id to return
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // the appointment id of the new row that was entered
            return appointmentId;
        }

        /// <summary>
        /// Save the appointment to the database for non logged in user
        /// </summary>
        /// <param name="appointment">Appointment object</param>
        /// <returns>The appoinmtentID of the appoitnemt what was just entered into the databse</returns>
        public int SaveAppointmentNonUser(AppointmentModel appointment)
        {
            int appointmentId = 0;

            try
            {
                // MY SQL statemnt
                string sqlStatement = "INSERT INTO appointments (serviceID, dateTime, vehicleYear, vehicleMake, vehicleModel, name, phoneNumber, address, zipCode, city, state, email) VALUES (@serviceID, @dateTime, @vehicleYear, @vehicleMake, @vehicleModel, @name, @phoneNumber, @address, @zipCode, @city, @state, @email); SELECT LAST_INSERT_ID();";// get id of the row of the new appoitnemtn insert

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
                    appointmentId = Convert.ToInt32(command.ExecuteScalar()); // store id to return
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // the appointment id of the new row that was entered
            return appointmentId;
        }

        /// <summary>
        /// Return the appointment based on the ID
        /// </summary>
        /// <param name="appointmentID">Appoitnment ID</param>
        /// <returns>Appointment Object found in the database</returns>
        public AppointmentModel GetAppointmentByID(int appointmentID)
        {
            // initialize
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
            // the found appoitnment
            return appointment;
        }
       


        /// <summary>
        /// Obtains all the appointments for a USER based on the ID of that user
        /// </summary>
        /// <param name="userID"> USER ID</param>
        /// <returns>List of appointments found for the user</returns>
        public List<AppointmentModel> GetAppointmentsByUserID(int userID)
        {
            // initialize list
            List<AppointmentModel> appointments = new List<AppointmentModel>();
            bool userAppointment = false;
            // MYSQL statement
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
                                // service info
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
                            // This is to get if the user was registered before the appoitnemnt was created
                            // obtains the non registered details over the user information
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
            // the list of user appointments
            return appointments;
        }

        /// <summary>
        /// This compares the email of a user with that of the email column of every appointment and sets the user ID in it
        /// This is so that the user can create an appoitnemnt with out logging in and see it under my appoitnemtns view
        /// </summary>
        /// <param name="email"> user email</param>
        /// <param name="userID">user id</param>
        public void UpdateAppointmentsWithUserID(string email, int userID)
        {
            // MYSQL Statement
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

        /// <summary>
        /// Sets appointments status to compelte in database
        /// </summary>
        /// <param name="appointmentID"> appointment id</param>
        public void CompleteAppointment(int appointmentID) 
        {
            // MYSQL statement
            // 3 = status id for complete in db
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

        /// <summary>
        /// Sets appointments status to Canceled in database
        /// </summary>
        /// <param name="appointmentID">appointment id </param>
        public void CancelAppointment(int appointmentID)
        {
            // MYSQL statement
            // 2 = status id for canceled in db
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

        /// <summary>
        /// returns if the name in the appointments table is null if the name column is null it returns true which means it is a user scheduled appoitnment 
        /// this is so that if a user registers after all creating an appoitnemnts he can see the details he used from when registered and not user details
        /// </summary>
        /// <param name="appointmentID"></param>
        /// <returns>if it is a user appoitnment true or false</returns>
        public bool IsUserAppointment(int appointmentID)
        {
            // sets to false to return if not user appointment
            bool isNull = false;
            try
            {
                // MYSQL statemtnt
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

        /// <summary>
        /// get all the appoitnemtns for the admin
        /// </summary>
        /// <returns>list of appointments</returns>
        public List<AppointmentModel> GetAllAppointments()
        {
            List<AppointmentModel> appointments = new List<AppointmentModel>();
            bool userAppointment = false;
            // join all tables associated to the appointments table
            // MYSQL statement only includes the most recent 2000
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
        LEFT JOIN
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
                                // service info
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
                            //user info
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
            // all appoitnemnts 
            return appointments;
        }

        // Old time slot information with out service input from user
        //public List<DateTime> GetAvailableTimeSlots(DateTime date)
        //{
        //    List<DateTime> availableSlots = new List<DateTime>();
        //    List<TimeSlot> bookedSlots = new List<TimeSlot>();

        //    try
        //    {
        //        // Retrieve all booked slots and their durations
        //        string sqlStatement = @"
        //    SELECT a.dateTime AS startTime, 
        //           s.duration
        //    FROM appointments a
        //    JOIN services s ON a.serviceID = s.serviceID
        //    WHERE DATE(a.dateTime) = @date
        //    ORDER BY startTime";

        //        using (MySqlConnection connection = new MySqlConnection(connectionString))
        //        {
        //            MySqlCommand command = new MySqlCommand(sqlStatement, connection);
        //            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

        //            connection.Open();
        //            MySqlDataReader reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                DateTime startTime = reader.GetDateTime("startTime");
        //                TimeSpan duration = reader.GetTimeSpan("duration"); // Assuming duration is stored as TimeSpan
        //                DateTime endTime = startTime.Add(duration);

        //                bookedSlots.Add(new TimeSlot { startTime = startTime, endTime = endTime });
        //            }
        //        }

        //        // Define working hours
        //        TimeSpan workingStartTime = new TimeSpan(8, 0, 0); // 8 AM
        //        TimeSpan workingEndTime = new TimeSpan(17, 0, 0);  // 5 PM

        //        // Generate all possible 30-minute slots within working hours
        //        DateTime currentSlotStart = date.Date.Add(workingStartTime);
        //        DateTime workEnd = date.Date.Add(workingEndTime);

        //        while (currentSlotStart < workEnd)
        //        {
        //            DateTime currentSlotEnd = currentSlotStart.AddMinutes(30);

        //            // Check if this slot overlaps with any booked slots
        //            bool isAvailable = true;
        //            foreach (var bookedSlot in bookedSlots)
        //            {
        //                if (!(currentSlotEnd <= bookedSlot.startTime || currentSlotStart >= bookedSlot.endTime))
        //                {
        //                    isAvailable = false;
        //                    break;
        //                }
        //            }

        //            // If the slot is available, add it to the list
        //            if (isAvailable)
        //            {
        //                availableSlots.Add(currentSlotStart);
        //            }

        //            // Move to the next 30-minute slot
        //            currentSlotStart = currentSlotEnd;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return availableSlots;
        //}




        /// <summary>
        /// Returns the avaible time slots based on scheduled appoitnemtns and the selected service of the user
        /// </summary>
        /// <param name="date"> selected date by user</param>
        /// <param name="serviceDuration"> th duration of the service selected by the user</param>
        /// <returns>list of timeslots avaible based on thje selection</returns>
        public List<DateTime> GetAvailableTimeSlots(DateTime date, TimeSpan serviceDuration)
        {
            List<DateTime> availableSlots = new List<DateTime>();
            List<TimeSlotModel> bookedSlots = new List<TimeSlotModel>();

            try
            {
                // Retrieve all booked slots and their durations
                string sqlStatement = @"
            SELECT a.dateTime AS startTime, 
                   s.duration
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
                        TimeSpan duration = reader.GetTimeSpan("duration"); 
                        DateTime endTime = startTime.Add(duration);
                        // add the timeslots to the list of booked appointments
                        bookedSlots.Add(new TimeSlotModel { startTime = startTime, endTime = endTime });
                    }
                }

                // Define working hours
                // may add this to be changed by the admin later
                TimeSpan workingStartTime = new TimeSpan(8, 0, 0); // 8 AM
                TimeSpan workingEndTime = new TimeSpan(17, 0, 0);  // 5 PM

                // Generate all possible slots within working hours
                DateTime currentSlotStart = date.Date.Add(workingStartTime);
                DateTime workEnd = date.Date.Add(workingEndTime);

                while (currentSlotStart.Add(serviceDuration) <= workEnd)
                {
                    DateTime currentSlotEnd = currentSlotStart.Add(serviceDuration);

                    // Check if this slot overlaps with any booked slots
                    bool isAvailable = true;
                    foreach (var bookedSlot in bookedSlots)
                    {
                        if (!(currentSlotEnd <= bookedSlot.startTime || currentSlotStart >= bookedSlot.endTime))
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    // If the slot is available, add it to the list
                    if (isAvailable)
                    {
                        availableSlots.Add(currentSlotStart);
                    }

                    // Move to the next 30-minute slot
                    currentSlotStart = currentSlotStart.AddMinutes(30);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return availableSlots;
        }

        


    }
}
