using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Diagnostics;

namespace FinalSDAProject
{
    public class DatabaseHelper
    {
        public static string connectionString = GetConnectionString();

        public static string GetConnectionString()
        {
            try
            {
                var conn = ConfigurationManager.ConnectionStrings["FlightBookingDB"];
                if (conn == null)
                {
                    throw new Exception("FlightBookingDB connection string not found in web.config");
                }
                Debug.WriteLine($"Using connection string: {conn.ConnectionString}");
                return conn.ConnectionString;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting connection string: {ex}");
                throw;
            }
        }


        public static bool ValidateUser(string username, string password)
        {
            Debug.WriteLine($"Validating user: {username}");

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        Debug.WriteLine($"Validation result for {username}: {count > 0}");

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Validation error: {ex.Message}");
                throw;
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
        public static DataRow GetUserDetails(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, Username, Email, FirstName, LastName, IsAdmin FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        conn.Open();
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                Debug.WriteLine($"Found user details for {username}");
                                return dt.Rows[0];
                            }
                            Debug.WriteLine($"No user found for {username}");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetUserDetails: {ex}");
                throw;
            }
        }


        // Flight Management
        public static DataTable GetFlights(string source, string destination, DateTime? departureDate)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        f.FlightID,
                        a.AirlineName,
                        a.LogoURL,
                        f.FlightNumber,
                        src.AirportName AS Source,
                        src.City AS SourceCity,
                        dest.AirportName AS Destination,
                        dest.City AS DestinationCity,
                        f.DepartureTime AS Departure,
                        f.ArrivalTime AS Arrival,
                        f.Price,
                        f.AvailableSeats
                    FROM Flights f
                    INNER JOIN Airlines a ON f.AirlineID = a.AirlineID
                    INNER JOIN Airports src ON f.SourceAirportID = src.AirportID
                    INNER JOIN Airports dest ON f.DestinationAirportID = dest.AirportID
                    WHERE (@Source IS NULL OR src.City LIKE '%' + @Source + '%' OR src.AirportName LIKE '%' + @Source + '%')
                    AND (@Destination IS NULL OR dest.City LIKE '%' + @Destination + '%' OR dest.AirportName LIKE '%' + @Destination + '%')
                    AND (@DepartureDate IS NULL OR CONVERT(DATE, f.DepartureTime) = @DepartureDate)
                    AND f.AvailableSeats > 0
                    ORDER BY f.DepartureTime";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Source", string.IsNullOrEmpty(source) ? (object)DBNull.Value : source);
                    cmd.Parameters.AddWithValue("@Destination", string.IsNullOrEmpty(destination) ? (object)DBNull.Value : destination);
                    cmd.Parameters.AddWithValue("@DepartureDate", departureDate.HasValue ? departureDate.Value.Date : (object)DBNull.Value);

                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public static DataTable GetAirports()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT AirportID, AirportName, City, Country, IATA_Code FROM Airports ORDER BY City, AirportName";
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Booking Management
        public static DataTable GetUserBookings(int userId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        b.BookingID,
                        a.AirlineName,
                        a.LogoURL,
                        f.FlightNumber,
                        src.AirportName AS Source,
                        dest.AirportName AS Destination,
                        f.DepartureTime AS Departure,
                        f.ArrivalTime AS Arrival,
                        b.TotalPrice AS Price,
                        b.Status,
                        b.BookingDate
                    FROM Bookings b
                    INNER JOIN Flights f ON b.FlightID = f.FlightID
                    INNER JOIN Airlines a ON f.AirlineID = a.AirlineID
                    INNER JOIN Airports src ON f.SourceAirportID = src.AirportID
                    INNER JOIN Airports dest ON f.DestinationAirportID = dest.AirportID
                    WHERE b.UserID = @UserID
                    ORDER BY f.DepartureTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public static bool BookFlight(int userId, int flightId, int passengers)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Get flight price
                    string priceQuery = "SELECT Price FROM Flights WHERE FlightID = @FlightID";
                    decimal price;
                    using (SqlCommand priceCmd = new SqlCommand(priceQuery, conn, transaction))
                    {
                        priceCmd.Parameters.AddWithValue("@FlightID", flightId);
                        price = Convert.ToDecimal(priceCmd.ExecuteScalar());
                    }

                    // Create booking
                    string bookingQuery = @"
                        INSERT INTO Bookings (UserID, FlightID, Passengers, TotalPrice)
                        VALUES (@UserID, @FlightID, @Passengers, @TotalPrice);
                        SELECT SCOPE_IDENTITY();";

                    int bookingId;
                    using (SqlCommand bookingCmd = new SqlCommand(bookingQuery, conn, transaction))
                    {
                        bookingCmd.Parameters.AddWithValue("@UserID", userId);
                        bookingCmd.Parameters.AddWithValue("@FlightID", flightId);
                        bookingCmd.Parameters.AddWithValue("@Passengers", passengers);
                        bookingCmd.Parameters.AddWithValue("@TotalPrice", price * passengers);

                        bookingId = Convert.ToInt32(bookingCmd.ExecuteScalar());
                    }

                    // Update available seats
                    string updateQuery = "UPDATE Flights SET AvailableSeats = AvailableSeats - @Passengers WHERE FlightID = @FlightID";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@FlightID", flightId);
                        updateCmd.Parameters.AddWithValue("@Passengers", passengers);
                        updateCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return bookingId > 0;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static bool CancelBooking(int bookingId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Get booking details
                    string detailsQuery = @"
                        SELECT b.FlightID, b.Passengers
                        FROM Bookings b
                        WHERE b.BookingID = @BookingID";

                    int flightId, passengers;
                    using (SqlCommand detailsCmd = new SqlCommand(detailsQuery, conn, transaction))
                    {
                        detailsCmd.Parameters.AddWithValue("@BookingID", bookingId);
                        using (SqlDataReader reader = detailsCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                throw new Exception("Booking not found");
                            }
                            flightId = reader.GetInt32(0);
                            passengers = reader.GetInt32(1);
                        }
                    }

                    // Update booking status
                    string updateBookingQuery = "UPDATE Bookings SET Status = 'Cancelled' WHERE BookingID = @BookingID";
                    using (SqlCommand updateBookingCmd = new SqlCommand(updateBookingQuery, conn, transaction))
                    {
                        updateBookingCmd.Parameters.AddWithValue("@BookingID", bookingId);
                        updateBookingCmd.ExecuteNonQuery();
                    }

                    // Restore seats
                    string restoreSeatsQuery = "UPDATE Flights SET AvailableSeats = AvailableSeats + @Passengers WHERE FlightID = @FlightID";
                    using (SqlCommand restoreCmd = new SqlCommand(restoreSeatsQuery, conn, transaction))
                    {
                        restoreCmd.Parameters.AddWithValue("@FlightID", flightId);
                        restoreCmd.Parameters.AddWithValue("@Passengers", passengers);
                        restoreCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Admin Functions


        public static bool AddUser(string username, string password, string email, string firstName, string lastName, bool isAdmin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Users (Username, Password, Email, FirstName, LastName, IsAdmin)
                    VALUES (@Username, @Password, @Email, @FirstName, @LastName, @IsAdmin)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }



        public static bool AddFlight(string flightNumber, int airlineId, int sourceAirportId, int destinationAirportId,
                               DateTime departureTime, DateTime arrivalTime, decimal price, int totalSeats,
                               int availableSeats, int duration)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO Flights (FlightNumber, AirlineID, SourceAirportID, DestinationAirportID, 
                                 DepartureTime, ArrivalTime, DurationMinutes, Price, TotalSeats, AvailableSeats)
            VALUES (@FlightNumber, @AirlineID, @SourceAirportID, @DestinationAirportID, 
                    @DepartureTime, @ArrivalTime, @Duration, 
                    @Price, @TotalSeats, @AvailableSeats)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FlightNumber", flightNumber);
                    cmd.Parameters.AddWithValue("@AirlineID", airlineId);
                    cmd.Parameters.AddWithValue("@SourceAirportID", sourceAirportId);
                    cmd.Parameters.AddWithValue("@DestinationAirportID", destinationAirportId);
                    cmd.Parameters.AddWithValue("@DepartureTime", departureTime);
                    cmd.Parameters.AddWithValue("@ArrivalTime", arrivalTime);
                    cmd.Parameters.AddWithValue("@Duration", duration);  
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@TotalSeats", totalSeats);
                    cmd.Parameters.AddWithValue("@AvailableSeats", availableSeats);  

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserId = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool DeleteFlight(int flightId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Flights WHERE FlightId = @FlightId", conn);
                cmd.Parameters.AddWithValue("@FlightId", flightId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool DeleteBooking(int bookingId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Bookings WHERE BookingId = @BookingId", conn);
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static DataTable GetAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetAllFlights()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Flights", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetAllBookings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Bookings", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        // Add these methods to your existing DatabaseHelper class

        public static bool UpdateUser(string username, string password, string firstName, string lastName, string email, bool isAdmin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Users SET 
                        Password = ISNULL(NULLIF(@Password, ''), Password),
                        FirstName = ISNULL(@FirstName, FirstName),
                        LastName = ISNULL(@LastName, LastName),
                        Email = ISNULL(@Email, Email),
                        IsAdmin = @IsAdmin
                        WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", string.IsNullOrEmpty(password) ? (object)DBNull.Value : password);
                    cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : firstName);
                    cmd.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                    cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public static DataTable GetAllAirlines()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AirlineID, AirlineName FROM Airlines", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static bool AddBooking(int userId, int flightId, DateTime bookingDate, int passengers, decimal totalPrice, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Bookings (UserID, FlightID, BookingDate, Passengers, TotalPrice, Status)
                        VALUES (@UserID, @FlightID, @BookingDate, @Passengers, @TotalPrice, @Status)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@FlightID", flightId);
                    cmd.Parameters.AddWithValue("@BookingDate", bookingDate);
                    cmd.Parameters.AddWithValue("@Passengers", passengers);
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}