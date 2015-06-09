namespace StudentFinanceSupport.Models
{
    using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

    public class ProfileUserViewModel
    {
        public StudentVoucher UserGrant { get; set; }
        public IEnumerable<StudentVoucher> UserGrantList { get; set; }
    }   
    public partial class StudentVoucher
    {
        [Key]
        public int id_student_vouchers { get; set; }

        [Required]
        [StringLength(20)]
        public string student_ID { get; set; }

        public int? grant_type_id { get; set; }

        [StringLength(50)]
        public string GrantDescription { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number")]
        public double GrantValue { get; set; }

        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfIssue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid number")]
        public double? KuhaFunds { get; set; }

       
        public virtual GrantType GrantType { get; set; }

        public virtual StudentRegistration StudentRegistration { get; set; }
    }
}
