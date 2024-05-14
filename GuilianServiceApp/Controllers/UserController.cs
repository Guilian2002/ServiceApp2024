using GuilianServiceApp.DAL.IDAL;
using GuilianServiceApp.Models;
using GuilianServiceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace GuilianServiceApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserDAL _user;

        public UserController(IUserDAL _user)
        {
            this._user = _user;
        }
        public IActionResult ToRegister()
        {
            return View();
        }
        public IActionResult Main()
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession))
            {
                TempData["Message"] = "User not connected!";
                return RedirectToAction("Home", "Index");
            }
            User user = JsonConvert.DeserializeObject<User>(userSession);

            ViewData["user"] = $"{user.Username}, here are your credits to accept a service if you wish : {user.Credits}";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToRegister(AddUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User(userViewModel);

                if (user.SaveUser(_user))
                    TempData["Message"] = "Account created successfully!";
                else
                    TempData["Message"] = "Error during the creation of your account! (Email or Username maybe already used)";
                return RedirectToAction("ToLogin", "User");
            }
            return View(userViewModel);
        }
        public IActionResult ToLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToLogin(string username, string password)
        {
            User user = Models.User.Login(username, password, _user);
            if (user is null)
            {
                return RedirectToAction("ToLogin", "User");
            }
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return RedirectToAction("Main", "User");
        }
        public IActionResult ToLogout()
        {
            HttpContext.Session.Clear();

            TempData["Message"] = "Disconnected Successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
