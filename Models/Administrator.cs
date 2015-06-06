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
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Password { get; set; }
        public int UserId { get; set; }
      
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
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Mobile Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^02[1-9]\\d{5,9}")]
        [StringLength(15)]
        public string mobile { get; set; }

        public AdministratorLogin loginDetails()
        {
            AdministratorLogin tmplogin = new AdministratorLogin();
            tmplogin.Email = this.Email;
            tmplogin.Password = this.Password;
            tmplogin.UserId = this.UserId;
            return tmplogin;
        }
        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Recovery> Recoveries { get; set; }

        
    }
}
