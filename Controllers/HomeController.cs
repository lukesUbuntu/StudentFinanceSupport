using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFinanceSupport.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
            bypassAdminCheck("About");
            bypassAdminCheck("Contact");
        }
        public ActionResult Index()
        {
            //return RedirectToRoute("Default");
            return View();
        }

        public ActionResult About()
        {
            //ViewBag.name = "";
            ViewBag.Message = "About Application.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}