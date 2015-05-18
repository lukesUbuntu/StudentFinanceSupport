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
        [Column(Order = 0)]
        [StringLength(20)]
        public string student_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string GrantType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string GrantDescription { get; set; }

        [Key]
        [Column(Order = 3)]
        public double GrantValue { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime DateOfIssue { get; set; }

        public double? KuhaFunds { get; set; }

        [Key]
        [Column(Order = 5)]
        public int Refno { get; set; }

        public virtual StudentRegistration StudentRegistration { get; set; }
    }
}
