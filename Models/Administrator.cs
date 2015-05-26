namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdministratorLogin
    {
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
    }
    public partial class Administrator
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public int? role_id { get; set; }

        public virtual Role Role { get; set; }
    }
}
