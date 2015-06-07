using StudentFinanceSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models.functions;
using System.Globalization;

namespace StudentFinanceSupport.Controllers
{
    public class ReportsController : BaseController
    {

        public ActionResult Index()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();

            ViewBag.Faculity = new MultiSelectList(db.Faculties, "id_faculty", "faculty_name");
            ViewBag.Campus = new MultiSelectList(db.Campus, "id_campus", "campus_name");
            ViewBag.GrantType = new MultiSelectList(db.GrantTypes, "grant_type_id", "grant_name");

            
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
        public ActionResult ReportFilter()
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
            ViewBag.Faculity = new MultiSelectList(db.Faculties, "id_faculty", "faculty_name");
            ViewBag.Campus = new MultiSelectList(db.Campus, "id_campus", "campus_name");
            ViewBag.GrantType = new MultiSelectList(db.GrantTypes, "grant_type_id", "grant_name");
            
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

        
        public JsonResult getFilterReports(Reports theReport)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            
            var student_grants = from student in db.StudentRegistrations 
                                 join grants in db.StudentVouchers on student.Student_ID equals grants.student_ID         
                                 select new
                                 {
                                   
                                     //GrantyType = grants.GrantType.grant_name,
                                     grant_type_id = grants.grant_type_id,
                                     //these are enabled values from grant types model
                                     grant_description_enabled = grants.GrantType.grant_description,
                                     grant_value_enabled = grants.GrantType.grant_value,
                                     grant_koha_enabled = grants.GrantType.grant_koha,

                                     //define all our returns that other filters may require
                                     Student_ID = grants.student_ID,
                                     Grant_ID = grants.id_student_vouchers,
                                     //due to cycler refrence we need to only specify data not reference to FKEY data
                                     Grant_Name = grants.GrantType.grant_name,

                                     GrantValue = grants.GrantValue,
                                     KohaFund = grants.KuhaFunds,
                                     DateOfIssue = grants.DateOfIssue,

                                     //From the students join
                                     FirstName = grants.StudentRegistration.FirstName,
                                     LastName = grants.StudentRegistration.LastName,
                                     Gender = grants.StudentRegistration.Gender,
                                     faculty_name = grants.StudentRegistration.Faculty.faculty_name,
                                     faculty_id = grants.StudentRegistration.Faculty.id_faculty,
                                     campus_id = grants.StudentRegistration.Campus.id_campus,
                                     campus_name = grants.StudentRegistration.Campus.campus_name
                                 };
            //apply our filters
            if (theReport.gender != null)
                student_grants = from a in student_grants where a.Gender.ToLower() == theReport.gender.ToLower() select a;

            if (theReport.date_type != null && theReport.start_date != null)
            {
                //our model will handle conversion
                DateTime theDate = theReport.getDate();

                 //DateTime theDate = new DateTime();
                //lets switch 
                switch(theReport.date_type.ToLower()){
                        //return results by day
                    case "day":
                        student_grants = from a in student_grants where a.DateOfIssue.Day == theDate.Day select a;
                        break;
                    case "week":
                        //calc between days
                        DateTime endDate = theDate;
                        endDate = endDate.AddDays(6);

                        student_grants = from a in student_grants 
                                         where a.DateOfIssue >= theDate
                                         && a.DateOfIssue < endDate
                                         select a;
                        break;
                    case "month":
                        student_grants = from a in student_grants 
                                         where a.DateOfIssue.Month == theDate.Month &&
                                         a.DateOfIssue.Year == theDate.Year
                                         select a;
                        break;
                    case "year":
                        student_grants = from a in student_grants 
                                         where a.DateOfIssue.Year == theDate.Year 
                                         select a;
                        break;
                }
                   
            }

            if (theReport.Faculity != null)
            {
                student_grants = from a in student_grants where a.faculty_id == theReport.Faculity select a;
            }
            //filter by grant type
            if (theReport.GrantType != null)
            {
                student_grants = from a in student_grants where a.grant_type_id == theReport.GrantType select a;
            }

            if (theReport.Campus != null)
            {
                student_grants = from a in student_grants where a.campus_id == theReport.Campus select a;
            }

            //finally grab the count of actually students for other chart
            var student_count = from a in student_grants group a by a.Student_ID into c select c;

            
            var grant_pie_report =
                  from voucher in student_grants.ToList()
                  where //voucher.GrantyType.grant_name.ToLower() != "advice"

                                    voucher.grant_description_enabled == true &&
                                    voucher.grant_koha_enabled == false &&
                                    voucher.grant_value_enabled == false

                  group voucher by voucher.Grant_Name into newGroup
                  select new
                  {

                      label = newGroup.Key,
                      data = newGroup.Sum(c => c.GrantValue)

                  };

            //report based on ONLY advice, which contains just a description field no koha or value
            var advice_report = from advice in student_grants
                                where
                                    advice.grant_description_enabled == true &&
                                    advice.grant_koha_enabled == false &&
                                    advice.grant_value_enabled == false
                                select advice;

            
            var faculty_pie_report =
                   from students in student_grants.ToList()
                   //where students.Grant_ID != advice_report.h 
                   group students by students.faculty_name into newGroup

                   select new
                   {

                       label = newGroup.Select(c => c.faculty_name).Distinct(),
                       data = newGroup.Sum(c => c.GrantValue)

                   };

           


            var main_report =
                from grant in student_grants
                //where grant.GrantyType.
                select new
                {
                    GrantType = grant.Grant_Name,
                    GrantValue = grant.GrantValue,
                    DateOfIssue = grant.DateOfIssue

                };
            
           
            return Json(new
            {
                success = true,
                student_grants = student_grants.ToList(),
                student_count = student_count.Count(),
                advice_count = advice_report.Count(),
                grant_pie_report = grant_pie_report,
                //value = newGroup.Sum(c => c.GrantValue)
                total_cost = student_grants.GroupBy(c => c.GrantValue).Select(c => c.Key).DefaultIfEmpty(0).Sum(),
                total_koha = student_grants.GroupBy(c => c.KohaFund).Select(c => c.Key).DefaultIfEmpty(0).Sum(),
               faculty_pie_report = faculty_pie_report,
               main_report = main_report.ToArray()
                
            }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult MainReport()
        //{
        //    StudentRegistrationsModel db = new StudentRegistrationsModel();
        //    //Get list of courses for a faculty

        //    //https://msdn.microsoft.com/en-us/library/bb545971.aspx
        //    /*
        //    var result = from voucher in db.StudentVouchers
        //                 group voucher by voucher.DateOfIssue into newGroup
        //                 select newGroup;
            
           
        //    var main_report1 = db.StudentVouchers.ToList() 
        //    .Select(grants =>
        //     new
        //     {
        //         GrantType = grants.GrantType,
        //         GrantValue = grants.GrantValue,
        //         DateOfIssue = grants.DateOfIssue
                
        //     });
        //     */
        //    var main_report = 
        //        from grant in db.StudentVouchers.ToList() where grant.GrantType.ToLower() != "advice"
        //        select new 
        //     {
        //         GrantType = grant.GrantType,
        //         GrantValue = grant.GrantValue,
        //         DateOfIssue = grant.DateOfIssue

        //     };

        //    var pie_chart_value =
        //           from voucher in db.StudentVouchers.ToList() where voucher.GrantType.ToLower() != "advice"
        //           group voucher by voucher.GrantType into newGroup

        //           select new
        //           {

        //               label = newGroup.Key,
        //               value = newGroup.Sum(c => c.GrantValue)

        //           };

        //    var pie_chart_faculty =
        //           from voucher in db.StudentVouchers.ToList()
        //           where voucher.GrantType.ToLower() != "advice"
        //           group voucher by voucher.StudentRegistration.Faculty into newGroup

        //           select new
        //           {

        //               label = newGroup.Key.faculty_name,
        //               value = newGroup.Sum(c => c.GrantValue)

        //           };

        //    var student_grants = from student in db.StudentRegistrations
        //                         join grants in db.StudentVouchers on student.Student_ID equals grants.student_ID
        //                         select new
        //                         {
        //                             Student_ID = grants.student_ID,
        //                             GrantyType = grants.GrantType,
        //                             GrantValue = grants.GrantValue,
        //                             DateOfIssue = grants.DateOfIssue,
        //                             FirstName = grants.StudentRegistration.FirstName,
        //                             LastName = grants.StudentRegistration.LastName,
        //                             Gender = grants.StudentRegistration.Gender,
        //                             faculty_name = grants.StudentRegistration.Faculty.faculty_name,
        //                         };
        //    //pie_chart_value = null;
        //   return Json(new
        //   {
        //       success = true,
        //       main_report = main_report,
        //       pie_chart_value = pie_chart_value,
        //       pie_chart_faculty = pie_chart_faculty,
        //       student_grants = student_grants
        //   }, JsonRequestBehavior.AllowGet);

        //    /*
        //     * x => new
        //          {
        //              id_course = x.id_courses,
        //              course_name = x.course_name
        //          });*/
        //    //int count = result.Count();

        //    //create a response back
        //    //var response = new List<object>();
        //    //response.Add(new { exists = (result.Count() == 1) });



        //   return Json(new
        //   {
        //       success = false
             
        //   }, JsonRequestBehavior.AllowGet);


        //}
    }
}