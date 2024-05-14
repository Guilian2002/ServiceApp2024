using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL
{
    public class FeedbackDAL : IFeedbackDAL
    {
        private readonly string connectionString;

        public FeedbackDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool SaveFeedback(Feedback feedback)
        {
            bool success = false;
            string query = "INSERT INTO [Feedback] (commentary, currentRating, FK_ActiveDuty) VALUES (@commentary, @currentRating, @FK_ActiveDuty)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@commentary", feedback.Commentary);
                cmd.Parameters.AddWithValue("@currentRating", feedback.Rating.ToString());
                cmd.Parameters.AddWithValue("@FK_ActiveDuty", feedback.ActiveDuty.Id);

                connection.Open();
                success = cmd.ExecuteNonQuery() > 0;
            }

            return success;
        }
    }
}
