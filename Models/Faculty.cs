namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Faculty")]
    public partial class Faculty
    {
        public Faculty()
        {
            Courses = new HashSet<Courses>();
            StudentRegistrations = new HashSet<StudentRegistration>();
        }

        [Key]
        public int id_faculty { get; set; }

        [StringLength(50)]
        public string faculty_name { get; set; }

        public virtual ICollection<Courses> Courses { get; set; }

        public virtual ICollection<StudentRegistration> StudentRegistrations { get; set; }
    }
}
