using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;
using GuilianServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GuilianServiceApp.Controllers
{
    public class ServiceProvidedController : Controller
    {
        private readonly IServiceProvidedDAL _serviceProvided;
        private readonly IUserDAL _user;
        public ServiceProvidedController(IServiceProvidedDAL _serviceProvided, IUserDAL _user)
        {
            this._serviceProvided = _serviceProvided;
            this._user = _user;
        }
        public IActionResult ToGiveServicesToUsers()
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connected to give a service!";
                return RedirectToAction("Home", "Index");
            }
            return View();
        }
        public IActionResult ToFindServices()
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                return View(ServiceProvided.GetAllServiceProvided(_serviceProvided));
            }
            User user = JsonConvert.DeserializeObject<User>(userSession);

            ViewData["user"] = $"{user.Username}, here are your credits to accept a service if you wish : {user.Credits}";
            return View(ServiceProvided.GetAllServiceProvidedByProvider(_serviceProvided, user));
        }
        public IActionResult Search(string keyword, int whichView)
        {
            if (whichView == 1)
            {
                List<ServiceProvided> serviceProvideds = ServiceProvided.GetAllServiceProvided(_serviceProvided);
                return View("ToFindServices", ServiceProvided.SearchServices(keyword, serviceProvideds));
            }
            string? userSession = HttpContext.Session.GetString("User");
            User user = JsonConvert.DeserializeObject<User>(userSession);
            List<ServiceProvided> serviceProvidedList = ServiceProvided.GetAllServiceProvidedByProvider
                (_serviceProvided, user);
            ViewData["user"] = user.Username;
            return View("ToFindServices", ServiceProvided.SearchServices(keyword, serviceProvidedList));
        }
        public IActionResult ToDetailService(int id)
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                return View(ServiceProvided.GetServiceProvidedById(_serviceProvided, id));
            }
            User user = JsonConvert.DeserializeObject<User>(userSession);

            ViewData["user"] = user.Username;
            return View(ServiceProvided.GetServiceProvidedById(_serviceProvided, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToGiveServicesToUsers(AddServiceProvidedViewModel serviceProvidedViewModel)
        {
            string? userSession = HttpContext.Session.GetString("User");
            User provider = JsonConvert.DeserializeObject<User>(userSession);
            if (ModelState.IsValid)
            {
                ServiceProvided serviceProvided = new ServiceProvided(serviceProvidedViewModel, provider);
                if (serviceProvided.SaveServiceProvided(_serviceProvided))
                    TempData["Message"] = "Service provided successfully!";
                else
                    TempData["Message"] = "Error during the creation of your service!";
                return RedirectToAction("Main", "User");
            }
            return View(serviceProvidedViewModel);
        }
    }
}
