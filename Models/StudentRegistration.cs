namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentRegistration")]
    public partial class StudentRegistration
    {
        public StudentRegistration()
        {
            StudentVouchers = new HashSet<StudentVoucher>();
        }

        [Key]
        [StringLength(20)]
        public string Student_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Fname { get; set; }

        [StringLength(20)]
        public string Lname { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Accomodition_Type { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Marital_Status { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [StringLength(50)]
        public string Main_Ethnicity { get; set; }

        public int id_faculty { get; set; }

        public int id_courses { get; set; }

        [StringLength(50)]
        public string Detailed_Ethnicity { get; set; }

        [Required]
        [StringLength(50)]
        public string campus { get; set; }

        public virtual Courses Course { get; set; }

        public virtual Faculty Faculty { get; set; }
        //Icollection is foreign key in vouchers table
        public virtual ICollection<StudentVoucher> StudentVouchers { get; set; }
    }
}
