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

        //admin and admin roles
        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }

        public virtual DbSet<StudentVoucher> StudentVouchers { get; set; }

        public virtual DbSet<Recovery> Recoveries { get; set; }

        public virtual DbSet<GrantType> GrantTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Administrator>()
                    .Property(e => e.Email)
                    .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Recovery>()
                .Property(e => e.recovery_key)
                .IsUnicode(false);

            modelBuilder.Entity<GrantType>()
              .Property(e => e.grant_name)
              .IsUnicode(false);

            modelBuilder.Entity<GrantType>()
                .HasMany(e => e.StudentVouchers)
                .WithRequired(e => e.GrantType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentVoucher>()
                .Property(e => e.student_ID)
                .IsUnicode(false);

            modelBuilder.Entity<StudentVoucher>()
                .Property(e => e.GrantDescription)
                .IsUnicode(false);
        }




    }
}
