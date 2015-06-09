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
        [Display(Name = "Student ID")]
        public string Student_ID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(20)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DOB { get; set; }

        [StringLength(50)]
        [Display(Name = "Address")]
        public string Address1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Accomodition")]
        public string Accomodition_Type { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Marital Status")]
        public string Marital_Status { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [StringLength(50)]
        [Display(Name = "Ethinicity")]
        public string Main_Ethnicity { get; set; }

        [StringLength(50)]
        public string Detailed_Ethnicity { get; set; }
       
        public int id_faculty { get; set; }
        
        public int id_courses { get; set; }
        
        public int id_campus { get; set; }

        
        public virtual Courses Course { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual Campus Campus { get; set; }

        //Icollection is foreign key in vouchers table
        public virtual ICollection<StudentVoucher> StudentVouchers { get; set; }

     
    }
}
