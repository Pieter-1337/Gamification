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

namespace Gamification.Controllers
{
    public class CountriesController : Controller
    {
        private GamificationEntities db = new GamificationEntities();
        private CountryRepository _countryRepository = null;

        public CountriesController()
        {
            _countryRepository = new CountryRepository();
        }

        // GET: Countries
        public ActionResult Index()
        {
            var CountryList = db.Countries.ToList();
            if(Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    int? SearchCountry = null;
                    if (Request["CountryID"] != null)
                    {
                        SearchCountry = Convert.ToInt32(Request["CountryID"]);
                    }

                    if(Request["SearchByCountryCheckBox"] == "check")
                    {
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        var searchResult = _countryRepository.SearchByCountry(CountryList, SearchCountry);
                        return View(searchResult);
                    }
                    else
                    {
                        ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name").OrderBy(c => c.Text);
                        return View(CountryList);
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

        // GET: Countries/Details/5
        public ActionResult Details(int? id)
        {
        var UserList = db.Users.ToList();
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Countries countries = db.Countries.Find(id);
                    if (countries == null)
                    {
                        return HttpNotFound();
                    }
                    return View(countries);
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

        // GET: Countries/Create
        public ActionResult Create()
        {
            var UserList = db.Users.ToList();
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
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

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryID,Name,Abbreviation")] Countries countries)
        {
            var UserList = db.Users.ToList();
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {

                    if (ModelState.IsValid)
                    {
                        db.Countries.Add(countries);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(countries);
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

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Countries countries = db.Countries.Find(id);
                    if (countries == null)
                    {
                        return HttpNotFound();
                    }
                    return View(countries);
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

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryID,Name,Abbreviation")] Countries countries)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(countries).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(countries);
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

        // GET: Countries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Countries countries = db.Countries.Find(id);
                    if (countries == null)
                    {
                        return HttpNotFound();
                    }
                    return View(countries);
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

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    Countries countries = db.Countries.Find(id);
                    db.Countries.Remove(countries);
                    db.SaveChanges();
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
