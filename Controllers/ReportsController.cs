using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFinanceSupport.Controllers
{
    public class ReportsController : BaseController
    {
       
        // GET: Reports
        public ActionResult Index()
        {
           
            return View();
        }
    }
}