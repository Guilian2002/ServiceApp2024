using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;
using GuilianServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace GuilianServiceApp.Controllers
{
    public class ActiveDutyController : Controller
    {
        private readonly IServiceProvidedDAL _serviceProvided;
        private readonly IActiveDutyDAL _activeDuty;
        private readonly IUserDAL _user;
        private readonly IFeedbackDAL _feedback;
        public ActiveDutyController(IActiveDutyDAL _activeDuty, IUserDAL _user, IFeedbackDAL _feedback, 
            IServiceProvidedDAL _serviceProvided)
        {
            this._activeDuty = _activeDuty;
            this._user = _user;
            this._feedback = _feedback;
            this._serviceProvided = _serviceProvided;
        }
        public IActionResult ToAcceptAService(int id)
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connect to accept a service!";
                return RedirectToAction("GetAllServiceProvided", "ServiceProvided");
            }
            ServiceProvided serviceProvided = ServiceProvided.GetServiceProvidedById(_serviceProvided, id);
            HttpContext.Session.SetString("ServiceProvided", JsonConvert.SerializeObject(serviceProvided));
            return View();
        }
        public IActionResult ToConsultServicesTaken()
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connected";
                return RedirectToAction("Index","Home");
            }
            User user = JsonConvert.DeserializeObject<User>(userSession);

            ViewData["user"] = user.Username;
            return View(ActiveDuty.GetActiveDutiesByRequester(_activeDuty, user));
        }
        public IActionResult Search(string keyword, int whichView)
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connected";
                return RedirectToAction("Index", "Home");
            }
            User user = JsonConvert.DeserializeObject<User>(userSession);
            if(whichView == 1)
            {
                List<ActiveDuty> activeDuties = ActiveDuty.GetActiveDutiesByRequester(_activeDuty, user);
                ViewData["user"] = user.Username;
                return View("ToConsultServicesTaken", ActiveDuty.SearchActiveDuties(keyword, activeDuties));
            }
            else if(whichView == 2)
            {
                Tuple<List<ActiveDuty>, List<ServiceProvided>> activeDutiesAndServices = ActiveDuty
                .GetActiveDutiesAndServicesByProvider(_activeDuty, user);
                List<ActiveDuty> activeDutyList = ActiveDuty.SearchActiveDuties
                    (keyword, activeDutiesAndServices.Item1);
                ViewData["user"] = user.Username;
                return View("ToConsultProvidedServices",
                    new Tuple<List<ActiveDuty>, List<ServiceProvided>>(activeDutyList, activeDutiesAndServices.Item2));
            }
            Tuple<List<ActiveDuty>, List<ServiceProvided>> activeDutyAndServiceList = ActiveDuty
                .GetActiveDutiesAndServicesByProvider(_activeDuty, user);
            List<ServiceProvided> serviceProvidedList = ServiceProvided.SearchServices
                (keyword, activeDutyAndServiceList.Item2);
            ViewData["user"] = user.Username;
            return View("ToConsultProvidedServices",
                new Tuple<List<ActiveDuty>, List<ServiceProvided>>(activeDutyAndServiceList.Item1,
                serviceProvidedList));
        }
        public IActionResult ToConsultProvidedServices()
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connected";
                return RedirectToAction("Index", "Home");
            }
            User user = JsonConvert.DeserializeObject<User>(userSession);

            ViewData["user"] = user.Username;
            return View(ActiveDuty.GetActiveDutiesAndServicesByProvider(_activeDuty, user));
        }
        public IActionResult ToDetailService(int id)
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connected";
                return RedirectToAction("Index", "Home");
            }
            return View(ActiveDuty.GetActiveDutyById(_activeDuty, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToAcceptAService(AddActiveDutyViewModel activeDutyViewModel)
        {
            if (ModelState.IsValid)
            {
                //Taking session objects
                string? userSession = HttpContext.Session.GetString("User");
                User requester = JsonConvert.DeserializeObject<User>(userSession);
                string? serviceProvidedSession = HttpContext.Session.GetString("ServiceProvided");
                ServiceProvided serviceProvided = JsonConvert.DeserializeObject<ServiceProvided>(serviceProvidedSession);

                ActiveDuty activeDuty = new ActiveDuty(serviceProvided, activeDutyViewModel, requester);
                //Verify if enough and remove credits for requester and add credits for provider
                if(activeDuty.RemoveCreditsForServiceTaken(_user))
                    if (activeDuty.SaveActiveDuty(_activeDuty))
                    {
                        /*Once the activeDuty has been changed the current user
                        lose credits and has to be save in session to update to correct his data*/
                        HttpContext.Session.SetString("User", JsonConvert.SerializeObject(activeDuty.Requester));
                        if (serviceProvided.ChangeStatus(_serviceProvided))
                            TempData["Message"] = "Service provided successfully!";
                        else
                            TempData["Message"] = "Error during the status change of the service!";
                    }
                    else
                        TempData["Message"] = "Error during the creation of your service!";
                else
                    TempData["Message"] = "An error occurred while creating your service! " +
                        "(You do not have sufficient credits for the requested hours?)";
                return RedirectToAction("ToFindServices", "ServiceProvided");
            }
            return View(activeDutyViewModel);
        }
        public IActionResult ChangeStatus(int id)
        {
            ActiveDuty activeDuty = ActiveDuty.GetActiveDutyById(_activeDuty, id);
            if(activeDuty.TransferCreditsForServiceFinished(_user))
                if (activeDuty.ChangeStatus(_activeDuty))
                    TempData["Message"] = "Service status changed successfully!";
                else
                    TempData["Message"] = "Error during the status change!";
            else
                TempData["Message"] = "Error during the transfer of credits!";
            return RedirectToAction("ToConsultServicesTaken", "ActiveDuty");
        }
    }
}
