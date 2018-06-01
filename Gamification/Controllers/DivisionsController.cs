using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gamification.Models;

namespace Gamification.Controllers
{
    public class DivisionsController : Controller
    {
        private GamificationEntities db = new GamificationEntities();

        // GET: Divisions
        public ActionResult Index()
        {
            return View(db.Divisions.ToList());
        }

        // GET: Divisions/Details/5
        public ActionResult Details(int? id)
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

        // GET: Divisions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DivisionID,Name,Abbreviation")] Divisions divisions)
        {
            if (ModelState.IsValid)
            {
                db.Divisions.Add(divisions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(divisions);
        }

        // GET: Divisions/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DivisionID,Name,Abbreviation")] Divisions divisions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(divisions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(divisions);
        }

        // GET: Divisions/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Divisions divisions = db.Divisions.Find(id);
            db.Divisions.Remove(divisions);
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
