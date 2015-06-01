using StudentFinanceSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentFinanceSupport.Controllers
{
    public class ReportsController : BaseController
    {

        public ActionResult Index()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();

            return View(db.StudentVouchers);
        }
        // GET: Reports
        public ActionResult ViewReport()
        {

            StudentRegistrationsModel db = new StudentRegistrationsModel();
            var result2 = from s in db.StudentVouchers select s.StudentRegistration.Faculty.faculty_name;
            

            var result =
            from s in db.StudentVouchers.Take(10)
             group s by s.DateOfIssue into g
             select new
             {
                 read_date = g.Key.Date,
                 GrantValue = g.Sum(x => x.GrantValue),
                 T2 = g.Select(x => x.GrantType),
             };
            
            return View(db.StudentVouchers.Take(10));
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
        public JsonResult getReport()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();





            return Json(new
            {
                success = false

            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MainReport()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            //Get list of courses for a faculty

            //https://msdn.microsoft.com/en-us/library/bb545971.aspx
            /*
            var result = from voucher in db.StudentVouchers
                         group voucher by voucher.DateOfIssue into newGroup
                         select newGroup;
            
           
            var main_report1 = db.StudentVouchers.ToList() 
            .Select(grants =>
             new
             {
                 GrantType = grants.GrantType,
                 GrantValue = grants.GrantValue,
                 DateOfIssue = grants.DateOfIssue
                
             });
             */
            var main_report = 
                from grant in db.StudentVouchers.ToList() where grant.GrantType.ToLower() != "advice"
                select new 
             {
                 GrantType = grant.GrantType,
                 GrantValue = grant.GrantValue,
                 DateOfIssue = grant.DateOfIssue

             };

            var pie_chart_value =
                   from voucher in db.StudentVouchers.ToList() where voucher.GrantType.ToLower() != "advice"
                   group voucher by voucher.GrantType into newGroup

                   select new
                   {

                       label = newGroup.Key,
                       value = newGroup.Sum(c => c.GrantValue)

                   };

            var pie_chart_faculty =
                   from voucher in db.StudentVouchers.ToList()
                   where voucher.GrantType.ToLower() != "advice"
                   group voucher by voucher.StudentRegistration.Faculty into newGroup

                   select new
                   {

                       label = newGroup.Key.faculty_name,
                       value = newGroup.Sum(c => c.GrantValue)

                   };

            var student_grants = from student in db.StudentRegistrations
                                 join grants in db.StudentVouchers on student.Student_ID equals grants.student_ID
                                 select new
                                 {
                                     Student_ID = grants.student_ID,
                                     GrantyType = grants.GrantType,
                                     GrantValue = grants.GrantValue,
                                     DateOfIssue = grants.DateOfIssue,
                                     FirstName = grants.StudentRegistration.FirstName,
                                     LastName = grants.StudentRegistration.LastName,
                                     Gender = grants.StudentRegistration.Gender,
                                     faculty_name = grants.StudentRegistration.Faculty.faculty_name,
                                 };
            //pie_chart_value = null;
           return Json(new
           {
               success = true,
               main_report = main_report,
               pie_chart_value = pie_chart_value,
               pie_chart_faculty = pie_chart_faculty,
               student_grants = student_grants
           }, JsonRequestBehavior.AllowGet);

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



           return Json(new
           {
               success = false
             
           }, JsonRequestBehavior.AllowGet);


        }
    }
}