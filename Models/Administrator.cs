namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //just for loging in purposes
    public partial class AdministratorLogin
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
    }
    public partial class Administrator
    {
        public Administrator()
        {
            Recoveries = new HashSet<Recovery>();
            Roles = new HashSet<Role>();
        }

        [Key]
        public int UserId { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
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

        public int? mobile { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Recovery> Recoveries { get; set; }
    }
}
