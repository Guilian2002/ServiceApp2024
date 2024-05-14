using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuilianServiceApp.Models
{
    public class ServiceProvided
    {
        private int id;
        private string title;
        private Status currentStatus;
        private string category;
        private string description;
        private User provider;

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
        public User Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        public ServiceProvided(){}

        public ServiceProvided(int id, string title, Status currentStatus, string category, string description, User provider)
        {
            this.id = id;
            this.title = title;
            this.currentStatus = currentStatus;
            this.category = category;
            this.description = description;
            this.provider = provider;
        }

        // Constructor for AddServiceProvidedViewModel
        public ServiceProvided(AddServiceProvidedViewModel addServiceProvidedViewModel, User provider)
        {
            this.title = addServiceProvidedViewModel.Title;
            this.currentStatus = Status.Pending;
            this.category = addServiceProvidedViewModel.Category;
            this.description = addServiceProvidedViewModel.Description;
            this.provider = provider;
        }
        public bool ChangeStatus(IServiceProvidedDAL serviceProvidedDAL)
        {
            switch (currentStatus)
            {
                case Status.Pending:
                    currentStatus = Status.InProgress;
                    return UpdateServiceProvidedStatus(serviceProvidedDAL);
                default:
                    currentStatus = Status.Completed;
                    return UpdateServiceProvidedStatus(serviceProvidedDAL);
            }
        }
        public static List<ServiceProvided> SearchServices(string keyword, List<ServiceProvided> serviceProvidedList)
        {
            keyword = keyword.ToLower();
            List<ServiceProvided> serviceList = serviceProvidedList
                .Where(s => s.Title.ToLower().Contains(keyword) || s.Category.ToLower().Contains(keyword) 
                || s.Provider.Username.ToLower().Contains(keyword))
                .ToList();
            return serviceList;
        }
        public bool SaveServiceProvided(IServiceProvidedDAL serviceProvidedDAL)
        {
            return serviceProvidedDAL.SaveServiceProvided(this);
        }
        public static List<ServiceProvided> GetAllServiceProvidedByProvider(IServiceProvidedDAL serviceProvidedDAL, User provider)
        {
            return serviceProvidedDAL.GetAllServiceProvidedByProvider(provider);
        }
        public static List<ServiceProvided> GetAllServiceProvided(IServiceProvidedDAL serviceProvidedDAL)
        {
            return serviceProvidedDAL.GetAllServiceProvided();
        }
        public bool UpdateServiceProvidedStatus(IServiceProvidedDAL serviceProvidedDAL)
        {
            return serviceProvidedDAL.UpdateServiceProvidedStatus(this);
        }
        public static ServiceProvided GetServiceProvidedById(IServiceProvidedDAL serviceProvidedDAL, int serviceProvidedId)
        {
            return serviceProvidedDAL.GetServiceProvidedById(serviceProvidedId);
        }
        public override bool Equals(object? obj)
        {
            return obj is ServiceProvided provided &&
                   id == provided.id &&
                   title == provided.title &&
                   currentStatus == provided.currentStatus &&
                   category == provided.category &&
                   description == provided.description &&
                   EqualityComparer<User>.Default.Equals(provider, provided.provider);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(id);
            hash.Add(title);
            hash.Add(currentStatus);
            hash.Add(category);
            hash.Add(description);
            hash.Add(provider);
            return hash.ToHashCode();
        }
        public override string ToString()
        {
            return $"ServiceProvided [Id={Id}, Title={Title}, CurrentStatus={CurrentStatus}, " +
                   $"Category={Category}, Description={Description}, Provider={Provider}]";
        }
    }
}
