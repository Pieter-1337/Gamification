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
                    int? SearchCountry = null;
                    int? SearchDivision = null;

                    if (Request["CountryID"] != null)
                    {
                        SearchCountry = Convert.ToInt32(Request["CountryID"]);
                    }


                    if (Request["DivisionID"] != null)
                    {
                        SearchDivision = Convert.ToInt32(Request["DivisionID"]);
                    }

                    var users = _userRepository.GetUsers();

                    //Search By Division
                    if (Request["SearchByDivisionCheckBox"] == "check")
                    {
                        ViewBag.RegisteredPeopleWholeApp = _userRepository.RegisteredPeopleWholeApp(users);
                        ViewBag.RegisteredPeoplePerCountry = _userRepository.RegisteredPeoplePerCountryByDivision(users, SearchDivision);
                        ViewBag.RegisteredPeoplePerDivision = _userRepository.RegisteredPeoplePerDivision(users);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        var SearchedDivision = (from d in db.Divisions where d.DivisionID == SearchDivision select d).FirstOrDefault();
                        TempData["PerCountryByDivision"] = $"You searched on: {SearchedDivision.Name}";
                        return View();
                    }

                    //Search By Country
                    if(Request["SearchByCountryCheckBox"] == "check")
                    {
                        ViewBag.RegisteredPeopleWholeApp = _userRepository.RegisteredPeopleWholeApp(users);
                        ViewBag.RegisteredPeoplePerCountry = _userRepository.RegisteredPeoplePerCountry(users);
                        ViewBag.RegisteredPeoplePerDivision = _userRepository.RegisteredPeoplePerDivisionByCountry(users, SearchCountry);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        var SearchedCountry = (from c in db.Countries where c.CountryID == SearchCountry select c).FirstOrDefault();
                        TempData["PerDivisionByCountry"] = $"You searched on: {SearchedCountry.Name}";
                        return View();
                    }

                    
                    //Query results for registered people All
                    ViewBag.RegisteredPeopleWholeApp = _userRepository.RegisteredPeopleWholeApp(users);
                    ViewBag.RegisteredPeoplePerCountry = _userRepository.RegisteredPeoplePerCountry(users);
                    ViewBag.RegisteredPeoplePerDivision = _userRepository.RegisteredPeoplePerDivision(users);
                   
                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
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

        public ActionResult BadgeIndex()
        {

            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    int? SearchCountry = null;
                    int? SearchDivision = null;

                    if (Request["CountryID"] != null)
                    {
                        SearchCountry = Convert.ToInt32(Request["CountryID"]);
                    }

                    if (Request["DivisionID"] != null)
                    {
                        SearchDivision = Convert.ToInt32(Request["DivisionID"]);
                    }
                    var users = _userRepository.GetUsers();

                    //Search by Division Level1
                    if(Request["SearchByDivisionLevel1CheckBox"] == "check")
                    {

                        ViewBag.BadgesEarnedWholeApp = _userRepository.BadgesEarnedWholeApp(users);
                        ViewBag.BadgesEarnedLevel1 = _userRepository.BadgesEarnedLevel1(users);
                        ViewBag.BadgesEarnedLevel1PerCountry = _userRepository.BadgesEarnedLevel1PerCountryPerDivision(users, SearchDivision);
                        ViewBag.BadgesEarnedLevel1PerDivision = _userRepository.BadgesEarnedLevel1PerDivision(users);
                        ViewBag.BadgesEarnedLevel2 = _userRepository.BadgesEarnedLevel2(users);
                        ViewBag.BadgesEarnedLevel2PerCountry = _userRepository.BadgesEarnedLevel2PerCountry(users);
                        ViewBag.BadgesEarnedLevel2PerDivision = _userRepository.BadgesEarnedLevel2PerDivision(users);

                        var SearchedDivision = (from d in db.Divisions where d.DivisionID == SearchDivision select d).FirstOrDefault();
                        TempData["PerCountryByDivisionLevel1"] = $"You searched on: {SearchedDivision.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }

                    //Search by Country Level1
                    if(Request["SearchByCountryLevel1CheckBox"] == "check")
                    {
                        ViewBag.BadgesEarnedWholeApp = _userRepository.BadgesEarnedWholeApp(users);
                        ViewBag.BadgesEarnedLevel1 = _userRepository.BadgesEarnedLevel1(users);
                        ViewBag.BadgesEarnedLevel1PerCountry = _userRepository.BadgesEarnedLevel1PerCountry(users);
                        ViewBag.BadgesEarnedLevel1PerDivision = _userRepository.BadgesEarnedLevel1PerDivisionPerCountry(users, SearchCountry);
                        ViewBag.BadgesEarnedLevel2 = _userRepository.BadgesEarnedLevel2(users);
                        ViewBag.BadgesEarnedLevel2PerCountry = _userRepository.BadgesEarnedLevel2PerCountry(users);
                        ViewBag.BadgesEarnedLevel2PerDivision = _userRepository.BadgesEarnedLevel2PerDivision(users);

                        var SearchedCountry = (from c in db.Countries where c.CountryID == SearchCountry select c).FirstOrDefault();
                        TempData["PerDivisionByCountryLevel1"] = $"You searched on: {SearchedCountry.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }

                    //Search by Division Level2
                    if(Request["SearchByDivisionLevel2CheckBox"] == "check")
                    {
                        ViewBag.BadgesEarnedWholeApp = _userRepository.BadgesEarnedWholeApp(users);
                        ViewBag.BadgesEarnedLevel1 = _userRepository.BadgesEarnedLevel1(users);
                        ViewBag.BadgesEarnedLevel1PerCountry = _userRepository.BadgesEarnedLevel1PerCountry(users);
                        ViewBag.BadgesEarnedLevel1PerDivision = _userRepository.BadgesEarnedLevel1PerDivision(users);
                        ViewBag.BadgesEarnedLevel2 = _userRepository.BadgesEarnedLevel2(users);
                        ViewBag.BadgesEarnedLevel2PerCountry = _userRepository.BadgesEarnedLevel2PerCountryPerDivision(users, SearchDivision);
                        ViewBag.BadgesEarnedLevel2PerDivision = _userRepository.BadgesEarnedLevel2PerDivision(users);

                        var SearchedDivision = (from d in db.Divisions where d.DivisionID == SearchDivision select d).FirstOrDefault();
                        TempData["PerCountryByDivisionLevel2"] = $"You searched on: {SearchedDivision.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }

                    //Search by country Level2
                    if(Request["SearchByCountryLevel2CheckBox"] == "check")
                    {
                        ViewBag.BadgesEarnedWholeApp = _userRepository.BadgesEarnedWholeApp(users);
                        ViewBag.BadgesEarnedLevel1 = _userRepository.BadgesEarnedLevel1(users);
                        ViewBag.BadgesEarnedLevel1PerCountry = _userRepository.BadgesEarnedLevel1PerCountry(users);
                        ViewBag.BadgesEarnedLevel1PerDivision = _userRepository.BadgesEarnedLevel1PerDivision(users);
                        ViewBag.BadgesEarnedLevel2 = _userRepository.BadgesEarnedLevel2(users);
                        ViewBag.BadgesEarnedLevel2PerCountry = _userRepository.BadgesEarnedLevel2PerCountry(users);
                        ViewBag.BadgesEarnedLevel2PerDivision = _userRepository.BadgesEarnedLevel2PerDivisionPerCountry(users, SearchCountry);

                        var SearchedCountry = (from c in db.Countries where c.CountryID == SearchCountry select c).FirstOrDefault();
                        TempData["PerDivisionByCountryLevel2"] = $"You searched on: {SearchedCountry.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }

                    //Query results for Badges earned by people
                    ViewBag.BadgesEarnedWholeApp = _userRepository.BadgesEarnedWholeApp(users);
                    ViewBag.BadgesEarnedLevel1 = _userRepository.BadgesEarnedLevel1(users);
                    ViewBag.BadgesEarnedLevel1PerCountry = _userRepository.BadgesEarnedLevel1PerCountry(users);
                    ViewBag.BadgesEarnedLevel1PerDivision = _userRepository.BadgesEarnedLevel1PerDivision(users);
                    ViewBag.BadgesEarnedLevel2 = _userRepository.BadgesEarnedLevel2(users);
                    ViewBag.BadgesEarnedLevel2PerCountry = _userRepository.BadgesEarnedLevel2PerCountry(users);
                    ViewBag.BadgesEarnedLevel2PerDivision = _userRepository.BadgesEarnedLevel2PerDivision(users);

                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
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

        public ActionResult PointsIndex()
        {

            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    int? SearchCountry = null;
                    int? SearchDivision = null;

                    if (Request["CountryID"] != null)
                    {
                        SearchCountry = Convert.ToInt32(Request["CountryID"]);
                    }

                    if (Request["DivisionID"] != null)
                    {
                        SearchDivision = Convert.ToInt32(Request["DivisionID"]);
                    }

                    var users = _userRepository.GetUsers();

                    //Search by Division Level1
                    if(Request["SearchByDivisionLevel1CheckBox"] == "check")
                    {
                        ViewBag.PointsEarnedWholeApp = _userRepository.PointsEarnedWholeApp(users);
                        ViewBag.PointsEarnedLevel1 = _userRepository.PointsEarnedLevel1(users);
                        ViewBag.PointsEarnedLevel2 = _userRepository.PointsEarnedLevel2(users);
                        ViewBag.PointsEarnedLevel1PerCountry = _userRepository.PointsEarnedLevel1PerCountryPerDivision(users, SearchDivision);
                        ViewBag.PointsEarnedLevel1PerDivision = _userRepository.PointsEarnedLevel1PerDivision(users);
                        ViewBag.PointsEarnedLevel2PerCountry = _userRepository.PointsEarnedLevel2PerCountry(users);
                        ViewBag.PointsEarnedLevel2PerDivision = _userRepository.PointsEarnedLevel2PerDivision(users);

                        var SearchedDivision = (from d in db.Divisions where d.DivisionID == SearchDivision select d).FirstOrDefault();
                        TempData["PerCountryByDivisionLevel1"] = $"You searched on: {SearchedDivision.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }

                    //Search by Country Level1
                    if(Request["SearchByCountryLevel1CheckBox"] == "check")
                    {
                        ViewBag.PointsEarnedWholeApp = _userRepository.PointsEarnedWholeApp(users);
                        ViewBag.PointsEarnedLevel1 = _userRepository.PointsEarnedLevel1(users);
                        ViewBag.PointsEarnedLevel2 = _userRepository.PointsEarnedLevel2(users);
                        ViewBag.PointsEarnedLevel1PerCountry = _userRepository.PointsEarnedLevel1PerCountry(users);
                        ViewBag.PointsEarnedLevel1PerDivision = _userRepository.PointsEarnedLevel1PerDivisionPerCountry(users, SearchCountry);
                        ViewBag.PointsEarnedLevel2PerCountry = _userRepository.PointsEarnedLevel2PerCountry(users);
                        ViewBag.PointsEarnedLevel2PerDivision = _userRepository.PointsEarnedLevel2PerDivision(users);

                        var SearchedCountry = (from c in db.Countries where c.CountryID == SearchCountry select c).FirstOrDefault();
                        TempData["PerDivisionByCountryLevel1"] = $"You searched on: {SearchedCountry.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }
                    //Search by Division Level2
                    if(Request["SearchByDivisionLevel2CheckBox"] == "check")
                    {
                        ViewBag.PointsEarnedWholeApp = _userRepository.PointsEarnedWholeApp(users);
                        ViewBag.PointsEarnedLevel1 = _userRepository.PointsEarnedLevel1(users);
                        ViewBag.PointsEarnedLevel2 = _userRepository.PointsEarnedLevel2(users);
                        ViewBag.PointsEarnedLevel1PerCountry = _userRepository.PointsEarnedLevel1PerCountry(users);
                        ViewBag.PointsEarnedLevel1PerDivision = _userRepository.PointsEarnedLevel1PerDivision(users);
                        ViewBag.PointsEarnedLevel2PerCountry = _userRepository.PointsEarnedLevel2PerCountryPerDivision(users, SearchDivision);
                        ViewBag.PointsEarnedLevel2PerDivision = _userRepository.PointsEarnedLevel2PerDivision(users);

                        var SearchedDivision = (from d in db.Divisions where d.DivisionID == SearchDivision select d).FirstOrDefault();
                        TempData["PerCountryByDivisionLevel2"] = $"You searched on: {SearchedDivision.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }

                    //Search by Country Level2
                    if (Request["SearchByCountryLevel2CheckBox"] == "check")
                    {
                        ViewBag.PointsEarnedWholeApp = _userRepository.PointsEarnedWholeApp(users);
                        ViewBag.PointsEarnedLevel1 = _userRepository.PointsEarnedLevel1(users);
                        ViewBag.PointsEarnedLevel2 = _userRepository.PointsEarnedLevel2(users);
                        ViewBag.PointsEarnedLevel1PerCountry = _userRepository.PointsEarnedLevel1PerCountry(users);
                        ViewBag.PointsEarnedLevel1PerDivision = _userRepository.PointsEarnedLevel1PerDivision(users);
                        ViewBag.PointsEarnedLevel2PerCountry = _userRepository.PointsEarnedLevel2PerCountry(users);
                        ViewBag.PointsEarnedLevel2PerDivision = _userRepository.PointsEarnedLevel2PerDivisionPerCountry(users, SearchCountry);

                        var SearchedCountry = (from c in db.Countries where c.CountryID == SearchCountry select c).FirstOrDefault();
                        TempData["PerDivisionByCountryLevel2"] = $"You searched on: {SearchedCountry.Name}";
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        return View();
                    }
                    //Query results for Points earned by people
                    ViewBag.PointsEarnedWholeApp = _userRepository.PointsEarnedWholeApp(users);
                    ViewBag.PointsEarnedLevel1 = _userRepository.PointsEarnedLevel1(users);
                    ViewBag.PointsEarnedLevel2 = _userRepository.PointsEarnedLevel2(users);
                    ViewBag.PointsEarnedLevel1PerCountry = _userRepository.PointsEarnedLevel1PerCountry(users);
                    ViewBag.PointsEarnedLevel1PerDivision = _userRepository.PointsEarnedLevel1PerDivision(users);
                    ViewBag.PointsEarnedLevel2PerCountry = _userRepository.PointsEarnedLevel2PerCountry(users);
                    ViewBag.PointsEarnedLevel2PerDivision = _userRepository.PointsEarnedLevel2PerDivision(users);

                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
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