namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrantType")]
    public partial class GrantType
    {
        public GrantType()
        {
            StudentVouchers = new HashSet<StudentVoucher>();
        }

        [Key]
        [Display(Name = "Grant Type")]
        public int grant_type_id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Grant Type")]
        public string grant_name { get; set; }

        [Display(Name = "Value Enabled")]
        public bool grant_value { get; set; }

        [Display(Name = "Description Enabled")]
        public bool grant_description { get; set; }

         [Display(Name = "Koha Enabled")]
        public bool grant_koha { get; set; }

         [Display(Name = "System Asset")]
        public bool system_asset { get; set; }

        public virtual ICollection<StudentVoucher> StudentVouchers { get; set; }
    }
}
