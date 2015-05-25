namespace StudentFinanceSupport.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudentRegistrationsModel : DbContext
    {
        public StudentRegistrationsModel()
            : base("name=StudentRegistrationsModel")
        {
        }
        public virtual DbSet<Campus> Campus { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public virtual DbSet<Administrator> Administrators { get; set; }

        public virtual DbSet<StudentVoucher> StudentVouchers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        
        }


    }
}
