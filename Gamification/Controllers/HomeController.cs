using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gamification.Models;
using Gamification.Repositorys;

namespace Gamification.Controllers
{
    public class HomeController : Controller
    {
        private UserRepository _userRepository = null;
        private CountryRepository _countryRepository = null;
        private DivisionRepository _divisionRepository = null;

        public HomeController()
        {
            _userRepository = new UserRepository();
            _countryRepository = new CountryRepository();
            _divisionRepository = new DivisionRepository();
        }

        public ActionResult Index()
        {
            _divisionRepository.LoadDivisions();
            _countryRepository.LoadCountries();
            _userRepository.LoadAdmins();
            var LeaderBoard = _userRepository.GetUsers().ToList();
            return View(LeaderBoard);
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("User");
            return RedirectToAction("Index", "Home", null);
        }

        [HttpPost]
        public ActionResult Bevestiging(string username, string password)
        {
            
            var user = _userRepository.GetUser(username, password);
            if (user != null)
            {
                Session["User"] = user;

                TempData["LoginValid"] = "Welcome " + user.First_Name + " " + user.Last_Name;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Login data was not correct please try again";
                return RedirectToAction("Login", "Home");
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}