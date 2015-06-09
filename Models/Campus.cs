namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    [Table("Campus")]
    public partial class Campus
    {
        public Campus()
        {
            StudentRegistrations = new HashSet<StudentRegistration>();
        }

        [Key]
        [Display(Name = "Campus")]
        public int id_campus { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Campus Name")]
        public string campus_name { get; set; }



        public virtual ICollection<StudentRegistration> StudentRegistrations { get; set; }
    }
}