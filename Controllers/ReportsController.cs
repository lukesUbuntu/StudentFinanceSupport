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
          
            //build list of issue with thereport filter model been passed


            string report_type = "Year To Date";

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
                                     DOB = grants.StudentRegistration.DOB,
                                     faculty_name = grants.StudentRegistration.Faculty.faculty_name,
                                     faculty_id = grants.StudentRegistration.Faculty.id_faculty,
                                     campus_id = grants.StudentRegistration.Campus.id_campus,
                                     campus_name = grants.StudentRegistration.Campus.campus_name,
                                     ethnicty = grants.StudentRegistration.Main_Ethnicity
                                 };
            //apply our filters
            if (theReport.gender != null)
                student_grants = from a in student_grants where a.Gender.ToLower() == theReport.gender.ToLower() select a;

            //by age
            if (theReport.student_age != null)
            {
                DateTime birthYear = theReport.getBirthYear();

                student_grants = from a in student_grants
                               //where clause by year of student
                               where a.DOB.Year == birthYear.Year
                               select a;

            }

            if (theReport.date_type != null)
            {
                // theReport.start_date != null
                //our model will handle conversion
                DateTime theDate = theReport.getDate();

                 //DateTime theDate = new DateTime();
                 //if empty datetype
                if (String.IsNullOrEmpty(theReport.start_date)){
                    ModelState.AddModelError("start_date", "Missing date");
                }
                else
                //lets switch 
                switch(theReport.date_type.ToLower()){
                        //return results by day
                    case "day":
                        student_grants = from a in student_grants where a.DateOfIssue.Day == theDate.Day select a;
                        report_type = "Daily Report";
                        break;
                    case "week":
                        //calc between days
                        DateTime endDate = theDate;
                        endDate = endDate.AddDays(6);

                        student_grants = from a in student_grants 
                                         where a.DateOfIssue >= theDate
                                         && a.DateOfIssue < endDate
                                         select a;
                        report_type = "Weekly Report";
                        break;
                    case "month":
                        student_grants = from a in student_grants 
                                         where a.DateOfIssue.Month == theDate.Month &&
                                         a.DateOfIssue.Year == theDate.Year
                                         select a;
                        report_type = "Monthly Report";
                        break;
                    case "year":
                        student_grants = from a in student_grants 
                                         where a.DateOfIssue.Year == theDate.Year 
                                         select a;
                        report_type = "Yearly Report";
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
            //etnicity filter
            if (theReport.Ethnicity != null)
            {
                bool is_Ethnicity = false;
                List<SelectListItem> EthnicityList = Helpers.Helpers.Ethnicity();

                foreach(var Ethnicity in EthnicityList)
                    if (Ethnicity.Value == theReport.Ethnicity) //we have ethncity
                        is_Ethnicity = true;
                        //student_grants = from a in student_grants where a.e == theReport.Faculity select a;
                
                if (is_Ethnicity)
                    student_grants = from a in student_grants where a.ethnicty == theReport.Ethnicity select a;
                else
                ModelState.AddModelError("Ethnicity", "invalid Ethnicity type");
                
                //student_grants = from a in student_grants where a.faculty_id == theReport.Faculity select a;
            }
            
            //finally grab the count of actually students for other chart
            var student_count = from a in student_grants group a by a.Student_ID into c select c;

            
            var grant_pie_report =
                  from voucher in student_grants.ToList()
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


            var campus_pie_report =
                   from students in student_grants.ToList()
                   //where students.Grant_ID != advice_report.h 
                   group students by students.campus_name into newGroup

                   select new
                   {

                       label = newGroup.Select(c => c.campus_name).Distinct(),
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
            //create our error list if any
            var errorList = ModelState.ToDictionary(
               kvp => kvp.Key,
               kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
              );
           
            return Json(new
            {
                success = true,
                report_type = report_type,
                student_grants = student_grants.ToList(),
                student_count = student_count.Count(),
                advice_count = advice_report.Count(),
                grant_pie_report = grant_pie_report,
                //value = newGroup.Sum(c => c.GrantValue)
                total_cost = student_grants.GroupBy(c => c.GrantValue).Select(c => c.Key).DefaultIfEmpty(0).Sum(),
                total_koha = student_grants.GroupBy(c => c.KohaFund).Select(c => c.Key).DefaultIfEmpty(0).Sum(),
                faculty_pie_report = faculty_pie_report,
                campus_pie_report = campus_pie_report,
                main_report = main_report.ToArray(),
                error_list = errorList.ToArray()
                
            }, JsonRequestBehavior.AllowGet);
        }
       
    }
}