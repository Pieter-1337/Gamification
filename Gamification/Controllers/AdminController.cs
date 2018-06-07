using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamification.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminUI()
        {
            if(Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if(user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Users");
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

        public ActionResult Statistics()
        {
            if (Session["User"] != null)
            {
                var user = (Gamification.Models.Users)Session["User"];
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Statistics");
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