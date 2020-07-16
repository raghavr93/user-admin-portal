using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAdminApi.Model;


namespace UserAdminApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>().HasData(
            new User
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@quaero.com",
                DateOfBirth = new DateTime(1, 1, 1),
                Address = "Admin Address",
                Password = "Admin@123",
                Role = "Admin"
            }
            );
        }

        public DbSet<User> Users { get; set; }
    }
}
