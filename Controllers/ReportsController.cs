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
            var result =
            from s in db.StudentVouchers.Take(10)
             group s by s.DateOfIssue into g
             select new
             {
                 read_date = g.Key.Date,
                 GrantValue = g.Sum(x => x.GrantValue),
                 T2 = g.Select(x => x.GrantType),
             };

            return View();
        }



        /*********AJAX ONLY TRIGGERS BELOW THIS POINT PUBLIC
        * @todo add token parsing for x-cross parsing security (cross site injections)
       /***********************************************************************************/
        /// <summary>
        /// Returns JSON response of all courses related to Faculty
        /// </summary>
        /// <param name="faculty">Faculity by ID</param>
        /// <returns>JSON result of id_course & course_name</returns>
        //StudentRegistration/getCourses [GET]
        public JsonResult getBarReport()
        {
            var result =
                    from voucher in db.StudentVouchers.ToList()
                    group voucher by voucher.GrantType into newGroup
                    
                    select new
                    {

                        label = newGroup.Key,
                        value = newGroup.Sum(c => c.GrantValue)
                        
                    };




            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MainReport()
        {
            //Get list of courses for a faculty

            //https://msdn.microsoft.com/en-us/library/bb545971.aspx
            /*
            var result = from voucher in db.StudentVouchers
                         group voucher by voucher.DateOfIssue into newGroup
                         select newGroup;
            
            */
           var result = db.StudentVouchers.ToList()
            .Select(grants =>
             new
             {
                 GrantType = grants.GrantType,
                 GrantValue = grants.GrantValue,
                 DateOfIssue = grants.DateOfIssue
             });
           

            /*
             * x => new
                  {
                      id_course = x.id_courses,
                      course_name = x.course_name
                  });*/
            //int count = result.Count();

            //create a response back
            //var response = new List<object>();
            //response.Add(new { exists = (result.Count() == 1) });



            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}