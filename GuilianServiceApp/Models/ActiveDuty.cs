using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace GuilianServiceApp.Models
{
    public class ActiveDuty
    {
        private int id;
        private string title;
        private Status currentStatus;
        private string category;
        private string description;
        private DateTime deadline;
        private int creditsHours;
        private User requester;
        private User provider;
        private Feedback feedback;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [Required(ErrorMessage = "Title Invalid."), StringLength(64, MinimumLength = 3, ErrorMessage = " Enter between 3 and 64 characters")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public Status CurrentStatus
        {
            get { return currentStatus; }
            set { currentStatus = value; }
        }
        [Required(ErrorMessage = "Category Invalid."), StringLength(64, MinimumLength = 3, ErrorMessage = " Enter between 3 and 64 characters")]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        [Required(ErrorMessage = "Description Invalid."), DataType(DataType.MultilineText)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
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
        public User Requester
        {
            get { return requester; }
            set { requester = value; }
        }
        public User Provider
        {
            get { return provider; }
            set { provider = value; }
        }
        public Feedback Feedback
        {
            get { return feedback; }
            set { feedback = value; }
        }

        public ActiveDuty() { }

        public ActiveDuty(int id, string title, Status currentStatus, string category, string description,
                          DateTime deadline, int creditsHours, User requester, User provider)
        {
            this.id = id;
            this.title = title;
            this.currentStatus = currentStatus;
            this.category = category;
            this.description = description;
            this.deadline = deadline;
            this.creditsHours = creditsHours;
            this.requester = requester;
            this.provider = provider;
        }
        public ActiveDuty(int id, string title, Status currentStatus, string category, string description,
                          DateTime deadline, int creditsHours, User requester, User provider, Feedback feedback)
        {
            this.id = id;
            this.title = title;
            this.currentStatus = currentStatus;
            this.category = category;
            this.description = description;
            this.deadline = deadline;
            this.creditsHours = creditsHours;
            this.requester = requester;
            this.provider = provider;
            this.feedback = feedback;
        }

        // Constructor for AddActiveDutyViewModel
        public ActiveDuty(ServiceProvided serviceProvided, AddActiveDutyViewModel activeDutyViewModel, User requester)
        {
            this.title = serviceProvided.Title;
            this.currentStatus = Status.InProgress;
            this.category = serviceProvided.Category;
            this.description = serviceProvided.Description;
            this.deadline = activeDutyViewModel.Deadline;
            this.creditsHours = activeDutyViewModel.CreditsHours;
            this.requester = requester;
            this.provider = serviceProvided.Provider;
        }
        private bool VerifyCreditsRequester()
        {
            return requester.Credits >= creditsHours;
        }
        public bool RemoveCreditsForServiceTaken(IUserDAL userDAL)
        {
            bool userAsEnoughCredits = VerifyCreditsRequester();
            if (userAsEnoughCredits)
            {
                requester.Credits -= creditsHours;  
                requester.UpdateCredits(userDAL);
            }
            return userAsEnoughCredits;
        }
        public bool TransferCreditsForServiceFinished(IUserDAL userDAL)
        {
            provider.Credits += creditsHours;
            bool success = provider.UpdateCredits(userDAL);
            return success;
        }
        public bool ChangeStatus(IActiveDutyDAL activeDutyDal)
        {
            bool success = false;
            switch(currentStatus)
            {
                case Status.Pending:
                    currentStatus = Status.InProgress;
                    success = UpdateActiveDutyStatus(activeDutyDal);
                    break;
                default:
                    currentStatus = Status.Completed;
                    success = UpdateActiveDutyStatus(activeDutyDal);
                    break;
            }
            return success;
        }
        public static List<ActiveDuty> SearchActiveDuties(string keyword, List<ActiveDuty> activeDutyList)
        {
            keyword = keyword.ToLower();
            List<ActiveDuty> activeDutiesList = activeDutyList
                .Where(a => a.Title.ToLower().Contains(keyword) || a.Category.ToLower().Contains(keyword) 
                || a.Provider.Username.ToLower().Contains(keyword) || a.Requester.Username.ToLower().Contains(keyword))
                .ToList();
            return activeDutiesList;
        }
        public bool SaveActiveDuty(IActiveDutyDAL activeDuty)
        {
            return activeDuty.SaveActiveDuty(this);
        }
        public static List<ActiveDuty> GetActiveDutiesByRequester(IActiveDutyDAL activeDutyDAL, User requester)
        {
            return activeDutyDAL.GetActiveDutiesByRequester(requester);
        }
        public static Tuple<List<ActiveDuty>, List<ServiceProvided>> GetActiveDutiesAndServicesByProvider
            (IActiveDutyDAL activeDutyDAL, User provider)
        {
            return activeDutyDAL.GetActiveDutiesAndServicesByProvider(provider);
        }
        public bool UpdateActiveDutyStatus(IActiveDutyDAL activeDutyDAL)
        {
            return activeDutyDAL.UpdateActiveDutyStatus(this);
        }
        public static ActiveDuty GetActiveDutyById(IActiveDutyDAL activeDutyDAL,int activeDutyId)
        {
            return activeDutyDAL.GetActiveDutyById(activeDutyId);
        }
        public override bool Equals(object? obj)
        {
            return obj is ActiveDuty duty &&
                   id == duty.id &&
                   title == duty.title &&
                   currentStatus == duty.currentStatus &&
                   category == duty.category &&
                   description == duty.description &&
                   deadline == duty.deadline &&
                   creditsHours == duty.creditsHours &&
                   EqualityComparer<User>.Default.Equals(requester, duty.requester) &&
                   EqualityComparer<User>.Default.Equals(provider, duty.provider) &&
                   EqualityComparer<Feedback>.Default.Equals(feedback, duty.feedback);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(id);
            hash.Add(title);
            hash.Add(currentStatus);
            hash.Add(category);
            hash.Add(description);
            hash.Add(deadline);
            hash.Add(creditsHours);
            hash.Add(requester);
            hash.Add(provider);
            hash.Add(feedback);
            return hash.ToHashCode();
        }
        public override string ToString()
        {
            return $"ActiveDuty [Id={Id}, Title={Title}, CurrentStatus={CurrentStatus}, " +
                   $"Category={Category}, Description={Description}, Deadline={Deadline}, " +
                   $"CreditsHours={CreditsHours}, Requester={Requester}, Provider={Provider}, " +
                   $"Feedback={Feedback}]";
        }
    }
}
