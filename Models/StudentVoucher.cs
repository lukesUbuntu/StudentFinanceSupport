namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentVoucher
    {
        

        [Key]
        public int id_student_vouchers { get; set; }

        [StringLength(20)]
        public string student_ID { get; set; }

       
        [StringLength(50)]
        [Required]
        public string GrantType { get; set; }

      

        public string GrantDescription { get; set; }


        public double GrantValue { get; set; }

        public DateTime DateOfIssue { get; set; }

        public double? KuhaFunds { get; set; }

        public virtual StudentRegistration StudentRegistration { get; set; }
    }
}
