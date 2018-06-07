using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gamification.Models;
using Gamification.Repositorys;
using BCrypt;
using System.Data.Entity.Validation;

namespace Gamification.Controllers
{
    public class UsersController : Controller
    {
        private GamificationEntities db = new GamificationEntities();
        private UserRepository _userRepository = null;

        public UsersController()
        {
            _userRepository = new UserRepository();
        }

        // GET: Users
        public ActionResult Index()
        {
            var UserList = db.Users.ToList();
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    string SearchString = null;
                    int? SearchCountry = null;
                    int? SearchDivision = null;

                    if (Request["SearchByNameTextBox"] != null)
                    {
                        SearchString = Request["SearchByNameTextBox"].ToString();
                    }
                   

                    if (Request["CountryID"] != null)
                    {
                        SearchCountry = Convert.ToInt32(Request["CountryID"]);
                    }
                    

                    if(Request["DivisionID"] != null)
                    {
                        SearchDivision = Convert.ToInt32(Request["DivisionID"]);
                    }
                    

                    //Search by Name/Username & Country & Division
                    if (Request["SearchByNameTextBox"] != null && Request["SearchByNameCheckBox"] == "check" && Request["SearchByCountryCheckBox"] == "check" && Request["SearchByDivisionCheckBox"] == "check")
                    {
                        var searchResult = _userRepository.GetLeaderBoardByNameAndCountryAndDivision(UserList, SearchString, SearchCountry, SearchDivision);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                    //Search by Name & Username & Country
                    if (Request["SearchByNameTextBox"] != null && Request["SearchByNameCheckBox"] == "check" && Request["SearchByCountryCheckBox"] == "check")
                    {
                        var searchResult = _userRepository.GetLeaderBoardByNameAndCountry(UserList, SearchString, SearchCountry);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                    //Search by Name/Username & Division
                    if (Request["SearchByNameTextBox"] != null && Request["SearchByNameCheckBox"] == "check" && Request["SearchByDivisionCheckBox"] == "check")
                    {
                        var searchResult = _userRepository.GetLeaderBoardByNameAndDivision(UserList, SearchString, SearchDivision);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                    //Search by Country & Division
                    if(Request["SearchByCountryCheckBox"] == "check" && Request["SearchByDivisionCheckBox"] == "check")
                    {
                        var searchResult = _userRepository.GetLeaderBoardByCountryAndDivision(UserList, SearchCountry, SearchDivision);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                    //Search by Division
                    if(Request["SearchByDivisionCheckBox"] == "check")
                    {
                        var searchResult = _userRepository.GetLeaderBoardByDivision(UserList, SearchDivision);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                 
                    //Search by Name & Username
                    if (Request["SearchByNameTextBox"] != null && Request["SearchByNameCheckBox"] == "check")
                    {

                        var searchResult = _userRepository.GetLeaderBoardByName(UserList, SearchString);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                    //Search by Country
                    if (Request["SearchByCountryCheckBox"] == "check")
                    {

                        var searchResult = _userRepository.GetLeaderBoardByCountry(UserList, SearchCountry);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        if (searchResult.Count() == 0)
                        {

                            TempData["UserListResult"] = "No results that match your search criteria where found";

                        }
                        return View(searchResult);
                    }

                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                    return View(db.Users.ToList());
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

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin" || user.UserID == Convert.ToInt32(id))
                {   
                    Users users = db.Users.Find(id);
                    if (users == null)
                    {
                        return HttpNotFound();
                    }
                    return View(users);
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

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,Email,First_Name,Last_Name,Punten_LVL1,Punten_LVL2,DivisionID,CountryID,Password,ConfirmPassword")] Users users)
        {
            if (users.Password == users.ConfirmPassword)
            {

                if (ModelState.IsValid)
                {
                    //Leave the password and username variable before the bcrypt otherwise the _userRepo.GetUser will fail
                    var password = users.Password;
                    var username = users.Username;

                    if (_userRepository.CheckUniqueFields(username, password))
                    {

                        users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);
                        users.ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(users.ConfirmPassword);
                        users.Role = "User";
                        db.Users.Add(users);
                        db.SaveChanges();

                        var user = _userRepository.GetUser(username, password);
                        var admin = (Gamification.Models.Users)Session["User"];
                        if (user != null && admin != null)
                        {
                            if(admin.Role != "Admin")
                            {

                                Session["User"] = user;
                                TempData["LoginValid"] = "Welcome " + user.First_Name + " " + user.Last_Name;
                                return RedirectToAction("Index", "Home", null);
                            }
                            else
                            {
                                TempData["UserCreated"] = "User " + user.First_Name + " " + user.Last_Name + " was succesfully created";
                                return RedirectToAction("Index", "Users", null);
                            }
                        }
                        else
                        {
                            Session["User"] = user;
                            TempData["LoginValid"] = "Welcome " + user.First_Name + " " + user.Last_Name;
                            return RedirectToAction("Index", "Home", null);
                        }
                    }
                    else
                    {
                        TempData["RegistrationValid"] = "This username is already in use please choose another username";
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        return View(users);
                    }
                }
            }
            else
            {
                TempData["RegistrationValid"] = "The password and Confirm password fields did not match please try again";
                ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                return View(users);
            }

            ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
            return View(users);
           
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin" || user.UserID == Convert.ToInt32(id))
                {
                    Users users = db.Users.Find(id);
                    if (users == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name", users.DivisionID).OrderBy(d => d.Text);
                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name", users.CountryID).OrderBy(c => c.Text);
                    ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name", users.BadgeID).OrderBy(b => b.Text);
                    return View(users);
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Email,First_Name,Last_Name,Punten_LVL1,Punten_LVL2,DivisionID,CountryID,BadgeID")] Users users)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin" || user.UserID == users.UserID)
                {
                    ModelState.Remove("Username");
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                    ModelState.Remove("Role");
                    var extraUserInfo = _userRepository.GetUserById(users.UserID);

                    if (ModelState.IsValid)
                    {
                        users.Username = extraUserInfo.Username;
                        users.Password = extraUserInfo.Password;
                        users.ConfirmPassword = extraUserInfo.ConfirmPassword;
                        users.Role = extraUserInfo.Role;

                        db.Entry(users).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            //For debugging reasons has no effect on the app
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                }
                            }

                            ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                            ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name").OrderBy(b => b.Text);
                            return View(users);
                        }
                        return RedirectToAction("Index");
                    }
                    ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(d => d.Text);
                    ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                    ViewBag.BadgeID = new SelectList(db.Badges, "BadgeID", "Name").OrderBy(b => b.Text);
                    return View(users);
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

        public ActionResult EditPassword(int? id)
        {
            if(Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = (Gamification.Models.Users)Session["User"];
                if(user.UserID == Convert.ToInt32(id))
                {
                    
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditPasswordConfirm(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = (Gamification.Models.Users)Session["User"];
                if (user.UserID == Convert.ToInt32(id))
                {
                    if((Request["OldPassword"] != null) && (Request["NewPassword"] != null))
                    {
                        if(_userRepository.UpdatePassword(Convert.ToInt32(id), Request["Oldpassword"].ToString(), Request["NewPassword"].ToString()))
                        {
                            TempData["PassChanged"] = "Password succesfully changed";
                            return RedirectToAction("Details", "Users", new { id = user.UserID}) ; //Update passed
                        }
                        else
                        {
                            TempData["PassChanged"] = "Password failed to Update";
                            return View();
                        }

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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
      
        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin" || user.UserID == Convert.ToInt32(id))
                {
                    Users users = db.Users.Find(id);
                    if (users == null)
                    {
                        return HttpNotFound();
                    }
                    return View(users);
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin" || user.UserID == Convert.ToInt32(id))
                {
                    Users users = db.Users.Find(id);
                    db.Users.Remove(users);
                    db.SaveChanges();
                    if (user.Role == "User")
                    {
                        Session.Remove("User");
                    }
                    return RedirectToAction("Index");
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
