using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL
{
    public class ServiceProvidedDAL : IServiceProvidedDAL
    {
        private readonly string connectionString;

        public ServiceProvidedDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool SaveServiceProvided(ServiceProvided serviceProvided)
        {
            bool success = false;
            string query = "INSERT INTO [ServiceProvided] (title, currentStatus, category, description, " +
                           "FK_Provider) " +
                           "VALUES (@title, @currentStatus, @category, @description, @FK_Provider)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@title", serviceProvided.Title);
                cmd.Parameters.AddWithValue("@currentStatus", serviceProvided.CurrentStatus.ToString());
                cmd.Parameters.AddWithValue("@category", serviceProvided.Category);
                cmd.Parameters.AddWithValue("@description", serviceProvided.Description);
                cmd.Parameters.AddWithValue("@FK_Provider", serviceProvided.Provider.Id);

                connection.Open();
                success = cmd.ExecuteNonQuery() > 0;
            }

            return success;
        }
        public List<ServiceProvided> GetAllServiceProvidedByProvider(User currentProvider)
        {
            List<ServiceProvided> serviceProvidedList = new List<ServiceProvided>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT sp.*, u.id as ProviderId, u.username, u.email, u.address " +
                               "FROM [ServiceProvided] sp " +
                               "INNER JOIN [User] u ON sp.FK_Provider = u.id " +
                               "WHERE currentStatus = 'Pending' and u.id != @providerId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@providerId", currentProvider.Id);

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User provider = new User
                            (
                                reader.GetInt32("ProviderId"),
                                reader.GetString("username"),
                                reader.GetString("address"),
                                reader.GetString("email")
                            );

                            ServiceProvided serviceProvided = new ServiceProvided
                            (
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                (Status)Enum.Parse(typeof(Status), reader.GetString("currentStatus")),
                                reader.GetString("category"),
                                reader.GetString("description"),
                                provider
                            );

                            serviceProvidedList.Add(serviceProvided);
                        }
                    }
                }
            }

            return serviceProvidedList;
        }
        public List<ServiceProvided> GetAllServiceProvided()
        {
            List<ServiceProvided> serviceProvidedList = new List<ServiceProvided>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT sp.*, u.id as ProviderId, u.username, u.email, u.address " +
                               "FROM [ServiceProvided] sp " +
                               "INNER JOIN [User] u ON sp.FK_Provider = u.id " +
                               "WHERE currentStatus = 'Pending'";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User provider = new User
                            (
                                reader.GetInt32("ProviderId"),
                                reader.GetString("username"),
                                reader.GetString("address"),
                                reader.GetString("email")
                            );

                            ServiceProvided serviceProvided = new ServiceProvided
                            (
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                (Status)Enum.Parse(typeof(Status), reader.GetString("currentStatus")),
                                reader.GetString("category"),
                                reader.GetString("description"),
                                provider
                            );

                            serviceProvidedList.Add(serviceProvided);
                        }
                    }
                }
            }

            return serviceProvidedList;
        }
        public bool UpdateServiceProvidedStatus(ServiceProvided serviceProvided)
        {
            bool success = false;

            string query = "UPDATE [ServiceProvided] SET currentStatus = @status WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@status", serviceProvided.CurrentStatus.ToString());
                cmd.Parameters.AddWithValue("@id", serviceProvided.Id);

                connection.Open();

                success = cmd.ExecuteNonQuery() > 0;
            }

            return success;
        }
        public ServiceProvided GetServiceProvidedById(int serviceProvidedId)
        {
            ServiceProvided serviceProvided = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT sp.*, u.id as ProviderId, u.username, u.email, u.address " +
                               "FROM [ServiceProvided] sp " +
                               "INNER JOIN [User] u ON sp.FK_Provider = u.id " +
                               "WHERE sp.id = @serviceProvidedId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@serviceProvidedId", serviceProvidedId);

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User provider = new User
                            (
                                reader.GetInt32("ProviderId"),
                                reader.GetString("username"),
                                reader.GetString("address"),
                                reader.GetString("email")
                            );

                            serviceProvided = new ServiceProvided
                            (
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                (Status)Enum.Parse(typeof(Status), reader.GetString("currentStatus")),
                                reader.GetString("category"),
                                reader.GetString("description"),
                                provider
                            );
                        }
                    }
                }
            }

            return serviceProvided;
        }
    }
}
