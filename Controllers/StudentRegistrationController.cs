using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models;
using System.Data.Entity.Validation;

namespace StudentFinanceSupport.Controllers
{
    
    public class StudentRegistrationController : BaseController
    {
        private StudentRegistrationsModel db = new StudentRegistrationsModel();

        // GET: StudentRegistration
        public ActionResult Index()
        {
            return View(db.StudentRegistrations.ToList());
        }

         public ActionResult Add()
        {
            return View();
        }

         //StudentRegistration/Create 
         public ActionResult Create()
         {
     

             ViewBag.id_courses = new SelectList(db.Courses, "id_courses", "course_name");

             ViewBag.id_faculty = new SelectList(db.Faculties, "id_faculty", "faculty_name");
             
             return View();
         }

         //StudentRegistration/Create [POST]
         [HttpPost]
         public ActionResult Create([Bind(Include =
             "Student_ID,Fname,Lname,Gender,DOB,Address1,Accomodition_Type,Phone,Mobile,Email,Marital_Status,Contact,Main_Ethnicity,id_faculty,id_courses,Detailed_Ethnicity,campus")] StudentRegistration studentRegistration)
         {

             //lets make sure we don't already have this Student_ID
             if (this.studentExists(studentRegistration.Student_ID))
             {
                 ModelState.AddModelError("Student_ID", "Student ID Already Exists");
                 
                 //return View(StudentRegistration);
             }
             
            
             if (ModelState.IsValid)
             {
                 
                 //administrator.UserType = "Admin";
                // db.Administrators.Add(administrator);

                 db.StudentRegistrations.Add(studentRegistration);
                 db.SaveChanges();
                 /*
                 try
                 {
                     
                     // Could also be before try if you know the exception occurs in SaveChanges

                   
                 }
                 catch (DbEntityValidationException e)
                 {
                     foreach (var eve in e.EntityValidationErrors)
                     {
                         System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                             eve.Entry.Entity.GetType().Name, eve.Entry.State);
                         foreach (var ve in eve.ValidationErrors)
                         {
                             System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                 ve.PropertyName,
                                 eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                 ve.ErrorMessage);
                         }
                     }
                     throw;
                 }
                 */
               
                 
                 return RedirectToAction("Index");
             }

             ViewBag.id_courses = new SelectList(db.Courses, "id_courses", "course_name");
             ViewBag.id_faculty = new SelectList(db.Faculties, "id_faculty", "faculty_name");
             return View(studentRegistration);
         }
        

        /// <summary>
        /// ajax call checks if a student id already exists
        /// </summary>
        /// <param name="Student_ID">The student ID</param>
        /// <returns>true or false</returns>
        //StudentRegistration/CheckStudentID [POST]
         [HttpPost]
         public JsonResult CheckStudentID(string Student_ID)
         {
             //search for student
             /*
             var result = (from r in db.StudentRegistrations
                           where r.Student_ID.ToLower().Equals(Student_ID.ToLower())
                           select new { r.Student_ID }).Distinct();
                          
             */
             //int count = result.Count();
             //create a response back
             //var response = new List<object>();
             //response.Add(new { exists = (result.Count() == 1) });

             List<object> response = new List<object>{
                                            //reverse result if not exist availble = true
                        new { available = (!this.studentExists(Student_ID)) }
             };
             return Json(response, JsonRequestBehavior.AllowGet);


         }

        /// <summary>
        /// Checks if a student exists 
        /// </summary>
        /// <param name="theStudentID">ID Of student</param>
        /// <returns>true or false if exists</returns>
         private bool studentExists(string theStudentID)
         {
             //lets make sure we don't already have this Student_ID .Any(). will return a boolean if the entity was found
             return (db.StudentRegistrations.Any(m => m.Student_ID.ToLower() == theStudentID.ToLower()));
         }
        
        /// <summary>
        /// Returns JSON response of all courses related to Faculty
        /// </summary>
        /// <param name="faculty">Faculity by ID</param>
         /// <returns>JSON result of id_course & course_name</returns>
         public JsonResult getCourses(int faculty)
         {
             //Get list of courses for a faculty
             var result = db.Courses.Where(x => x.id_faculty == faculty).Select(x => new
                  {
                      id_course = x.id_courses,
                      course_name = x.course_name
                  });

             //int count = result.Count();
             //create a response back
             //var response = new List<object>();
             //response.Add(new { exists = (result.Count() == 1) });

             
             
             return Json(result, JsonRequestBehavior.AllowGet);


         }
    }
}