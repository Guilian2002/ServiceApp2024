using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly string connectionString;

        public UserDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public User Login(string username, string password)
        {
            User u = null;
            string query = "SELECT * FROM [User] where username = @username and password = @password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        u = new User(reader.GetInt32("id"), reader.GetString("firstname"),
                                     reader.GetString("lastname"), reader.GetString("username"),
                                     reader.GetString("address"), reader.GetString("email"),
                                     null, reader.GetInt32("credits"));
                    }
                }
            }
            return u;
        }

        public bool UserAlreadyExist(User u)
        {
            bool success = false;

            string query = "SELECT * FROM [User] WHERE username = @username OR email = @email";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("username", u.Username);
                cmd.Parameters.AddWithValue("email", u.Email);
                connection.Open();

                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }

        public bool SaveUser(User u)
        {
            bool success = false;
            string query = "INSERT INTO [User](lastname, firstname, username, password, email, address, credits)" +
                            " VALUES (@lastname, @firstname, @username, @password, @email, @address, @credits) ";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("lastname", u.Lastname);
                cmd.Parameters.AddWithValue("firstname", u.Firstname);
                cmd.Parameters.AddWithValue("username", u.Username);
                cmd.Parameters.AddWithValue("password", u.Password);
                cmd.Parameters.AddWithValue("email", u.Email);
                cmd.Parameters.AddWithValue("address", u.Address);
                cmd.Parameters.AddWithValue("credits", u.Credits);
                connection.Open();
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }
        public bool UpdateCredits(User u)
        {
            bool success = false;
            string query = "Update [User] SET credits = @credits WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("credits", u.Credits);
                cmd.Parameters.AddWithValue("id", u.Id);
                connection.Open();
                success = cmd.ExecuteNonQuery() > 0;
            }
            return success;
        }
    }
}
