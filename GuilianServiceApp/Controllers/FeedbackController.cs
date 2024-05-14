using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;
using GuilianServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GuilianServiceApp.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackDAL _feedback;
        private readonly IActiveDutyDAL _activeDuty;
        public FeedbackController(IFeedbackDAL _feedback, IActiveDutyDAL _activeDuty)
        {
            this._feedback = _feedback;
            this._activeDuty = _activeDuty;
        }
        public IActionResult ToReviewCompletedServices(int id)
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connect to review the service!";
                return RedirectToAction("ToConsultServicesTaken", "ActiveDuty");
            }
            ActiveDuty activeDuty = ActiveDuty.GetActiveDutyById(_activeDuty, id);
            HttpContext.Session.SetString("ActiveDuty", JsonConvert.SerializeObject(activeDuty));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToReviewCompletedServices(AddFeedbackViewModel feedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                string? activeDutySession = HttpContext.Session.GetString("ActiveDuty");
                ActiveDuty activeDuty = JsonConvert.DeserializeObject<ActiveDuty>(activeDutySession);

                Feedback feedback = new Feedback(feedbackViewModel, activeDuty);

                if (feedback.SaveFeedback(_feedback))
                    TempData["Message"] = "Feedback sent successfully!";
                else
                    TempData["Message"] = "Error during the creation of your feedback!";
                return RedirectToAction("ToConsultServicesTaken", "ActiveDuty");
            }
            return View(feedbackViewModel);
        }
    }
}
