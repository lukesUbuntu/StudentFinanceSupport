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

        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<StudentRegistration> StudentRegistrations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Courses>()
                .Property(e => e.course_name)
                .IsUnicode(false);

            modelBuilder.Entity<Courses>()
                .HasMany(e => e.StudentRegistrations)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Faculty>()
                .Property(e => e.faculty_name)
                .IsUnicode(false);

            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Faculty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.StudentRegistrations)
                .WithRequired(e => e.Faculty)
                .WillCascadeOnDelete(false);

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
        }
    }
}
