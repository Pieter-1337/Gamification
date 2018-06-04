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
        private GamificationEntities db = new GamificationEntities();
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
            var LeaderBoard = _userRepository.GetLeaderBoard().ToList();
            string SearchString = null;
            int? SearchCountry = null;

            if (Request["SearchByNameTextBox"] != null)
            {
                SearchString = Request["SearchByNameTextBox"].ToString();
            }

            if (Request["CountryID"] != null)
            {
                SearchCountry = Convert.ToInt32(Request["CountryID"]);
            }

            //Search by Name & Username & Country
            if (Request["SearchByNameTextBox"] != null && Request["SearchByNameCheckBox"] == "check" && Request["SearchByCountryCheckBox"] == "check")
            {
                var searchResult = _userRepository.GetLeaderBoardByNameAndCountry(LeaderBoard, SearchString, SearchCountry);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                return View(searchResult);
            }

            //Search by Name & Username
            if(Request["SearchByNameTextBox"] != null && Request["SearchByNameCheckBox"] == "check")
            {

                var searchResult = _userRepository.GetLeaderBoardByName(LeaderBoard, SearchString);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                return View(searchResult);
            }

            //Search by Country
            if (Request["SearchByCountryCheckBox"] == "check")
            {

                var searchResult = _userRepository.GetLeaderBoardByCountry(LeaderBoard, SearchCountry);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                return View(searchResult);
            }

            //Get all users for Leaderboard
            LeaderBoard = _userRepository.GetLeaderBoard().ToList();
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
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