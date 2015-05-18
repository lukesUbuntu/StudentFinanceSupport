namespace StudentFinanceSupport.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AdministratorsModel : DbContext
    {
        public AdministratorsModel()
            : base("name=AdministratorsModel")
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; }

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

            modelBuilder.Entity<Administrator>()
                .Property(e => e.UserType)
                .IsUnicode(false);
        }
    }
}
