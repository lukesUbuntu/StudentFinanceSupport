namespace StudentFinanceSupport.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudentVouchersModel : DbContext
    {
        public StudentVouchersModel()
            : base("name=StudentVouchersModel")
        {
        }

        public virtual DbSet<StudentRegistration> StudentRegistrations { get; set; }
        public virtual DbSet<StudentVoucher> StudentVouchers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Student_ID)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Fname)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Lname)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Accomodition_Type)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Marital_Status)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Contact)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Main_Ethnicity)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.Detailed_Ethnicity)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .Property(e => e.campus)
                .IsUnicode(false);

            modelBuilder.Entity<StudentRegistration>()
                .HasMany(e => e.StudentVouchers)
                .WithRequired(e => e.StudentRegistration)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentVoucher>()
                .Property(e => e.student_ID)
                .IsUnicode(false);

            modelBuilder.Entity<StudentVoucher>()
                .Property(e => e.GrantType)
                .IsUnicode(false);

            modelBuilder.Entity<StudentVoucher>()
                .Property(e => e.GrantDescription)
                .IsUnicode(false);
        }
    }
}
