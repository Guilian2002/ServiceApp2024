using GuilianServiceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace GuilianServiceApp.ViewModels
{
    public class AddServiceProvidedViewModel
    {
        private string title;
        private string category;
        private string description;
        private User provider;

        [Required(ErrorMessage = "Title Invalid."), StringLength(64, MinimumLength = 3, ErrorMessage = " Enter between 3 and 64 characters")]
        public string Title
        {
            get { return title; }
            set { title = value; }
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
    }
}
