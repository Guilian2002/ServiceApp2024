using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace GuilianServiceApp.Models
{
    public class Feedback
    {
        private int id;
        private string commentary;
        private Rating rating;
        private ActiveDuty activeDuty;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [Required(ErrorMessage = "Commentary Invalid."), DataType(DataType.MultilineText)]
        public string Commentary
        {
            get { return commentary; }
            set { commentary = value; }
        }
        public Rating Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public ActiveDuty ActiveDuty
        {
            get { return activeDuty; }
            set { activeDuty = value; }
        }

        public Feedback(){}

        public Feedback(int id, string commentary, Rating rating, ActiveDuty activeDuty)
        {
            this.id = id;
            this.commentary = commentary;
            this.rating = rating;
            this.activeDuty = activeDuty;
        }

        //Constructor for AddFeedbackViewModel
        public Feedback(AddFeedbackViewModel feedbackViewModel, ActiveDuty activeDuty)
        {
            commentary = feedbackViewModel.Commentary;
            rating = feedbackViewModel.Rating;
            this.activeDuty = activeDuty;
        }
        
        public bool SaveFeedback(IFeedbackDAL feedbackDAL)
        {
            return feedbackDAL.SaveFeedback(this);
        }
        public override bool Equals(object? obj)
        {
            return obj is Feedback feedback &&
                   id == feedback.id &&
                   commentary == feedback.commentary &&
                   rating == feedback.rating &&
                   EqualityComparer<ActiveDuty>.Default.Equals(activeDuty, feedback.activeDuty);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, commentary, rating, activeDuty);
        }
        public override string ToString()
        {
            return $"Feedback [Id={Id}, Commentary={Commentary}, Rating={Rating}, ActiveDuty={ActiveDuty}]";
        }
    }
}
