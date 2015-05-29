namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recovery")]
    public partial class Recovery
    {
        [Key]
        public int recovery_id { get; set; }

        public int? UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string recovery_key { get; set; }

        public virtual Administrator Administrator { get; set; }
    }
}
