namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.Spatial;

    [Table("Courses")]
    public partial class Courses
    {
        public Courses()
        {
            StudentRegistrations = new HashSet<StudentRegistration>();
        }

        [Key]
        [Display(Name = "Course")]
        public int id_courses { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Course Name")]
        public string course_name { get; set; }
         [Display(Name = "Faculty")]
        public int id_faculty { get; set; }

        public virtual Faculty Faculty { get; set; }
        //foreign key
        public virtual ICollection<StudentRegistration> StudentRegistrations { get; set; }
    }
}
