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
        public int grant_type_id { get; set; }

        [Required]
        [StringLength(50)]
        public string grant_name { get; set; }

        public bool grant_value { get; set; }

        public bool grant_description { get; set; }

        public bool grant_koha { get; set; }

        public bool system_asset { get; set; }

        public virtual ICollection<StudentVoucher> StudentVouchers { get; set; }
    }
}
