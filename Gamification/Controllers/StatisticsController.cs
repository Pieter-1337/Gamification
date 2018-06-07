using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gamification.Models;
using Gamification.Repositorys;


namespace Gamification.Controllers
{
    public class StatisticsController : Controller
    {
        private GamificationEntities db = new GamificationEntities();
        private UserRepository _userRepository = null;

        public StatisticsController()
        {
            _userRepository = new UserRepository();
        }

        // GET: Statistics
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    ViewBag.RegisteredPeopleWholeApp = _userRepository.RegisteredPeopleWholeApp();
                    ViewBag.RegisteredPeoplePerCountry = _userRepository.RegisteredPeoplePerCountry();
                    ViewBag.RegisteredPeoplePerDivision = _userRepository.RegisteredPeoplePerDivision();

                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}