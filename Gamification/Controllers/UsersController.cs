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
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
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
        public ActionResult Create([Bind(Include = "UserID,Username,Email,First_Name,Last_Name,Punten_LVL1,Punten_LVL2,Division,Country,Password,ConfirmPassword")] Users users)
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
                        users.Role = "User";
                        db.Users.Add(users);
                        db.SaveChanges();

                        var user = _userRepository.GetUser(username, password);
                        if (user != null)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Email,First_Name,Last_Name,Punten_LVL1,Punten_LVL2,Division,Country,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
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
