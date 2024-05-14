using GuilianServiceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace GuilianServiceApp.ViewModels
{
    public class AddFeedbackViewModel
    {
        private string commentary;
        private Rating rating;
        private int activeDutyId;


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
    }
}
