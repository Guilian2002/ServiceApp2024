using GuilianServiceApp.Models;

namespace GuilianServiceApp.DAL.IDAL
{
    public interface IFeedbackDAL
    {
        public bool SaveFeedback(Feedback feedback);
    }
}
