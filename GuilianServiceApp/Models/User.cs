using System.ComponentModel.DataAnnotations;
using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.ViewModels;

namespace GuilianServiceApp.Models
{
    public class User
    {
        private int id;
        private string firstname;
        private string lastname;
        private string username;
        private string email;
        private string password;
        private string address;
        private int credits;
        private List<ServiceProvided> serviceProvidedList;
        private List<ActiveDuty> activeDutyList;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [Required(ErrorMessage = "Firstname Invalid."), StringLength(32, MinimumLength = 3, ErrorMessage = " Enter between 3 and 32 characters")]
        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }
        [Required(ErrorMessage = "Lastname Invalid."), StringLength(32, MinimumLength = 3, ErrorMessage = " Enter between 3 and 32 characters")]
        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }
        [Required(ErrorMessage = "Username Invalid."), StringLength(32, MinimumLength = 3, ErrorMessage = " Enter between 3 and 32 characters")]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        [Required(ErrorMessage = "Address Invalid."), StringLength(128, MinimumLength = 8, ErrorMessage = " Enter between 8 and 128 characters")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        [Required(ErrorMessage = "Email Invalid!"), DataType(DataType.EmailAddress)]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        [Required(ErrorMessage = "Password Invalid!"), DataType(DataType.Password)]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public int Credits
        {
            get { return credits; }
            set { credits = value; }
        }
        public List<ServiceProvided> ServiceProvidedList
        {
            get { return serviceProvidedList; }
            set { serviceProvidedList = value; }
        }
        public List<ActiveDuty> ActiveDutyList
        {
            get { return activeDutyList; }
            set { activeDutyList = value; }
        }

        public User(){}
        public User(int id, string firstname, string lastname, string username, string address, string email, 
                    string password, int credits)
        {
            this.id = id;
            this.firstname = firstname;
            this.lastname = lastname;
            this.username = username;
            this.address = address;
            this.email = email;
            this.password = password;
            this.credits = credits;
            serviceProvidedList = new List<ServiceProvided>();
            activeDutyList = new List<ActiveDuty>();
        }
        public User(int id, string username, string address, string email)
        {
            this.id = id;
            this.username = username;
            this.address = address;
            this.email = email;
            serviceProvidedList = new List<ServiceProvided>();
            activeDutyList = new List<ActiveDuty>();
        }
        public User(int id, string username, string address, string email, int credits)
        {
            this.id = id;
            this.username = username;
            this.address = address;
            this.email = email;
            this.credits = credits;
            serviceProvidedList = new List<ServiceProvided>();
            activeDutyList = new List<ActiveDuty>();
        }
        public User(int id, string firstname, string lastname, string username, string address, string email,
                    string password, int credits, List<ServiceProvided> serviceProvidedList, List<ActiveDuty> activeDutyList)
        {
            this.id = id;
            this.firstname = firstname;
            this.lastname = lastname;
            this.username = username;
            this.address = address;
            this.email = email;
            this.password = password;
            this.credits = credits;
            this.serviceProvidedList = serviceProvidedList;
            this.activeDutyList = activeDutyList;
        }

        // View Model User Constructor
        public User(AddUserViewModel userViewModel)
        {
            firstname = userViewModel.Firstname;
            lastname = userViewModel.Lastname;
            username = userViewModel.Username;
            address = userViewModel.Address;
            email = userViewModel.Email;
            password = userViewModel.Password;
            credits = 10;
            serviceProvidedList = new List<ServiceProvided>();
            activeDutyList = new List<ActiveDuty>();
        }
        public static User Login(string username, string password, IUserDAL userDAL)
        {
            return userDAL.Login(username, password);
        }
        public bool SaveUser(IUserDAL userDAL)
        {
            bool find = userDAL.UserAlreadyExist(this);
            if(find == false)
                return userDAL.SaveUser(this);
            else 
                return find;
        }
        public bool UpdateCredits(IUserDAL userDAL)
        {
            return userDAL.UpdateCredits(this);
        }
        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   id == user.id &&
                   firstname == user.firstname &&
                   lastname == user.lastname &&
                   username == user.username &&
                   email == user.email &&
                   password == user.password &&
                   address == user.address &&
                   credits == user.credits &&
                   EqualityComparer<List<ServiceProvided>>.Default.Equals(serviceProvidedList, user.serviceProvidedList) &&
                   EqualityComparer<List<ActiveDuty>>.Default.Equals(activeDutyList, user.activeDutyList);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(id);
            hash.Add(firstname);
            hash.Add(lastname);
            hash.Add(username);
            hash.Add(email);
            hash.Add(password);
            hash.Add(address);
            hash.Add(credits);
            hash.Add(serviceProvidedList);
            hash.Add(activeDutyList);
            return hash.ToHashCode();
        }
        public override string ToString()
        {
            return $"User [Id={Id}, Firstname={Firstname}, Lastname={Lastname}, Username={Username}," +
                   $" Email={Email}, Password={Password}, Address={Address}, Credits={Credits}," +
                   $" ServiceProvidedList={ServiceProvidedList}, ActiveDutyList={ActiveDutyList}]";
        }
    }
}
