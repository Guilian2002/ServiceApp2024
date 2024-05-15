using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL
{
    public class ActiveDutyDAL : IActiveDutyDAL
    {
        private readonly string connectionString;

        public ActiveDutyDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool SaveActiveDuty(ActiveDuty activeDuty)
        {
            bool success = false;
            string query = "INSERT INTO [ActiveDuty] (title, currentStatus, category, description, deadline," +
                           " creditsHours, FK_Requester, FK_Provider) " +
                           "VALUES (@title, @currentStatus, @category, @description, @deadline, @creditsHours," +
                           " @FK_Requester, @FK_Provider)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@title", activeDuty.Title);
                cmd.Parameters.AddWithValue("@currentStatus", activeDuty.CurrentStatus.ToString());
                cmd.Parameters.AddWithValue("@category", activeDuty.Category);
                cmd.Parameters.AddWithValue("@description", activeDuty.Description);
                cmd.Parameters.AddWithValue("@deadline", activeDuty.Deadline);
                cmd.Parameters.AddWithValue("@creditsHours", activeDuty.CreditsHours);
                cmd.Parameters.AddWithValue("@FK_Requester", activeDuty.Requester.Id);
                cmd.Parameters.AddWithValue("@FK_Provider", activeDuty.Provider.Id);

                connection.Open();
                success = cmd.ExecuteNonQuery() > 0;
            }

            return success;
        }

        public List<ActiveDuty> GetActiveDutiesByRequester(User requester)
        {
            List<ActiveDuty> activeDutyList = new List<ActiveDuty>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ad.*, u.id as ProviderId, u.username, u.email, u.address, " +
                       "fb.id as FeedbackId, fb.commentary, fb.currentRating " +
                       "FROM [ActiveDuty] ad " +
                       "INNER JOIN [User] u ON ad.FK_Provider = u.id " +
                       "LEFT JOIN [Feedback] fb ON ad.id = fb.FK_ActiveDuty " +
                       "WHERE ad.FK_Requester = @requesterId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    Feedback feedback = new Feedback();
                    cmd.Parameters.AddWithValue("@requesterId", requester.Id);

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
                            if (!reader.IsDBNull("FeedbackId"))
                            {
                                feedback = new Feedback
                                (
                                    reader.GetInt32(reader.GetOrdinal("FeedbackId")),
                                    reader.GetString("commentary"),
                                    (Rating)Enum.Parse(typeof(Rating), reader.GetString("currentRating")),
                                    null
                                );
                            }
                            ActiveDuty activeDuty = new ActiveDuty
                            (
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                (Status)Enum.Parse(typeof(Status), reader.GetString("currentStatus")),
                                reader.GetString("category"),
                                reader.GetString("description"),
                                reader.GetDateTime("deadline"),
                                reader.GetInt32("creditsHours"),
                                requester,
                                provider,
                                feedback
                            );

                            activeDutyList.Add(activeDuty);
                        }
                    }
                }
            }

            return activeDutyList;
        }
        public Tuple<List<ActiveDuty>, List<ServiceProvided>> GetActiveDutiesAndServicesByProvider(User provider)
        {
            List<ActiveDuty> activeDutyList = new List<ActiveDuty>();
            List<ServiceProvided> serviceProvidedList = new List<ServiceProvided>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string activeDutyQuery = "SELECT ad.*, u.id as RequesterId, u.username, u.email, u.address " +
                                         "FROM [ActiveDuty] ad " +
                                         "INNER JOIN [User] u ON ad.FK_Requester = u.id " +
                                         "WHERE ad.FK_Provider = @providerId";

                string serviceProvidedQuery = "SELECT sp.* FROM [ServiceProvided] sp " +
                                              "WHERE sp.FK_Provider = @providerId and sp.currentStatus='Pending'";

                using (SqlCommand cmd = new SqlCommand(activeDutyQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@providerId", provider.Id);

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User requester = new User
                            (
                                reader.GetInt32("RequesterId"),
                                reader.GetString("username"),
                                reader.GetString("address"),
                                reader.GetString("email")
                            );

                            ActiveDuty activeDuty = new ActiveDuty
                            (
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                (Status)Enum.Parse(typeof(Status), reader.GetString("currentStatus")),
                                reader.GetString("category"),
                                reader.GetString("description"),
                                reader.GetDateTime("deadline"),
                                reader.GetInt32("creditsHours"),
                                requester,
                                provider
                            );

                            activeDutyList.Add(activeDuty);
                        }
                    }
                }
                using (SqlCommand cmd = new SqlCommand(serviceProvidedQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@providerId", provider.Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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

            return new Tuple<List<ActiveDuty>, List<ServiceProvided>>(activeDutyList, serviceProvidedList);
        }
        public bool UpdateActiveDutyStatus(ActiveDuty activeDuty)
        {
            bool success = false;

            string query = "UPDATE [ActiveDuty] SET currentStatus = @status WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@status", activeDuty.CurrentStatus.ToString());
                cmd.Parameters.AddWithValue("@id", activeDuty.Id);

                connection.Open();

                success = cmd.ExecuteNonQuery() > 0;
            }

            return success;
        }
        public ActiveDuty GetActiveDutyById(int activeDutyId)
        {
            Feedback feedback = null;
            ActiveDuty activeDuty = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ad.*, u1.id as RequesterId, u1.username as RequesterUsername, " +
                               "u1.email as RequesterEmail, u1.address as RequesterAddress, " +
                               "u1.credits as RequesterCredits, u2.id as ProviderId, " +
                               "u2.username as ProviderUsername, u2.email as ProviderEmail, " +
                               "u2.address as ProviderAddress, u2.credits as ProviderCredits, f.id as FeedbackId, " +
                               "f.commentary as FeedbackCommentary, f.currentRating as FeedbackCurrentRating " +
                               "FROM [ActiveDuty] ad " +
                               "INNER JOIN [User] u1 ON ad.FK_Requester = u1.id " +
                               "INNER JOIN [User] u2 ON ad.FK_Provider = u2.id " +
                               "LEFT JOIN [Feedback] f ON ad.id = f.FK_ActiveDuty " +
                               "WHERE ad.id = @activeDutyId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@activeDutyId", activeDutyId);

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User requester = new User
                            (
                                reader.GetInt32("RequesterId"),
                                reader.GetString("RequesterUsername"),
                                reader.GetString("RequesterAddress"),
                                reader.GetString("RequesterEmail"),
                                reader.GetInt32("RequesterCredits")
                            );

                            User provider = new User
                            (
                                reader.GetInt32("ProviderId"),
                                reader.GetString("ProviderUsername"),
                                reader.GetString("ProviderAddress"),
                                reader.GetString("ProviderEmail"),
                                reader.GetInt32("ProviderCredits")
                            );

                            if (!reader.IsDBNull("FeedbackId"))
                            {
                                feedback = new Feedback
                                (
                                    reader.GetInt32(reader.GetOrdinal("FeedbackId")),
                                    reader.GetString("FeedbackCommentary"),
                                    (Rating)Enum.Parse(typeof(Rating), reader.GetString("FeedbackCurrentRating")),
                                    null
                                );
                            }
                            activeDuty = new ActiveDuty
                            (
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                (Status)Enum.Parse(typeof(Status), reader.GetString("currentStatus")),
                                reader.GetString("category"),
                                reader.GetString("description"),
                                reader.GetDateTime("deadline"),
                                reader.GetInt32("creditsHours"),
                                requester,
                                provider,
                                feedback
                            );
                        }
                    }
                }
            }

            return activeDuty;
        }
    }
}
