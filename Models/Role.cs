namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role
    {
        public Role()
        {
            Administrators = new HashSet<Administrator>();
        }

        [Key]
        public int role_id { get; set; }

        public int UserId { get; set; }

        public int role_type_id { get; set; }

        public virtual ICollection<Administrator> Administrators { get; set; }

        public virtual RoleType RoleType { get; set; }
    }
}
