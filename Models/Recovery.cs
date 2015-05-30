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
        /*
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        [StringLength(100)]
        public string Email { get; set; }*/
        public int? UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string recovery_key { get; set; }

        [StringLength(20)]
        public string recovery_option { get; set; }
        public virtual Administrator Administrator { get; set; }



    }
}
