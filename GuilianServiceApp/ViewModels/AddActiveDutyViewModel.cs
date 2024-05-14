using GuilianServiceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace GuilianServiceApp.ViewModels
{
    public class AddActiveDutyViewModel
    {
        private DateTime deadline;
        private int creditsHours;

        [Required(ErrorMessage = "Deadline Invalid."), DataType(DataType.DateTime)]
        public DateTime Deadline
        {
            get { return deadline; }
            set { deadline = value; }
        }
        [Required(ErrorMessage = "Invalid number of hours"), Range(1, 12, ErrorMessage = " Enter number of hours between 1 and 12")]
        public int CreditsHours
        {
            get { return creditsHours; }
            set { creditsHours = value; }
        }
    }
}
