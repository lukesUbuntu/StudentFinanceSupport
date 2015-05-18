namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Courses")]
    public partial class Courses
    {
        public Courses()
        {
            StudentRegistrations = new HashSet<StudentRegistration>();
        }

        [Key]
        public int id_courses { get; set; }

        [Required]
        [StringLength(100)]
        public string course_name { get; set; }

        public int id_faculty { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual ICollection<StudentRegistration> StudentRegistrations { get; set; }
    }
}
