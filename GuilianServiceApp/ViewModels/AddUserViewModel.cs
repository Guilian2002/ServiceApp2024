using System.ComponentModel.DataAnnotations;
using System.Net;
using GuilianServiceApp.Models;

namespace GuilianServiceApp.ViewModels
{
    public class AddUserViewModel
    {
        // ViewModel pour create account
        private string firstname;
        private string lastname;
        private string username;
        private string address;
        private string email;
        private string password;

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
    }
}
