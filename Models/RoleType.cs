namespace StudentFinanceSupport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleType
    {
        public RoleType()
        {
            Administrators = new HashSet<Administrator>();
        }

        [Key]
        public int role_type_id { get; set; }

        [StringLength(20)]
        public string role_name { get; set; }

        [StringLength(200)]
        public string role_description { get; set; }

        public virtual ICollection<Administrator> Administrators { get; set; }
    }
}
