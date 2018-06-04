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
    public class DivisionsController : Controller
    {
        private GamificationEntities db = new GamificationEntities();
        private DivisionRepository _divisionRepository = null;

        public DivisionsController()
        {
            _divisionRepository = new DivisionRepository();
        }

        // GET: Divisions
        public ActionResult Index()
        {
            var DivisionList = db.Divisions.ToList();
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    int? SearchDivision = null;
                    if (Request["DivisionID"] != null)
                    {
                        SearchDivision = Convert.ToInt32(Request["DivisionID"]);
                    }

                    if (Request["SearchByDivisionCheckBox"] == "check")
                    {
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(c => c.Text);
                        var searchResult = _divisionRepository.SearchByDivision(DivisionList, SearchDivision);
                        return View(searchResult);
                    }
                    else
                    {
                        ViewBag.DivisionID = new SelectList(db.Divisions, "DivisionID", "Name").OrderBy(c => c.Text);
                        return View(DivisionList);
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

        // GET: Divisions/Details/5
        public ActionResult Details(int? id)
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
                    Divisions divisions = db.Divisions.Find(id);
                    if (divisions == null)
                    {
                        return HttpNotFound();
                    }
                    return View(divisions);
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

        // GET: Divisions/Create
        public ActionResult Create()
        {
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

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DivisionID,Name,Abbreviation")] Divisions divisions)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        db.Divisions.Add(divisions);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(divisions);
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

        // GET: Divisions/Edit/5
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
                    Divisions divisions = db.Divisions.Find(id);
                    if (divisions == null)
                    {
                        return HttpNotFound();
                    }
                    return View(divisions);
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

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DivisionID,Name,Abbreviation")] Divisions divisions)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(divisions).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(divisions);
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

        // GET: Divisions/Delete/5
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
                    Divisions divisions = db.Divisions.Find(id);
                    if (divisions == null)
                    {
                        return HttpNotFound();
                    }
                    return View(divisions);
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

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    Divisions divisions = db.Divisions.Find(id);
                    db.Divisions.Remove(divisions);
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
