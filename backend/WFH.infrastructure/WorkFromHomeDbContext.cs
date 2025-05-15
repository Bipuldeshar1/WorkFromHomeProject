
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WFH.infrastructure;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Infrastructure
{
    public class WorkFromHomeDbContext : IdentityDbContext<AppUser>
    {
        public WorkFromHomeDbContext(DbContextOptions<WorkFromHomeDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<EmployeeRequest> EmployeeRequests { get; set; }

        public DbSet<ActiveEmployees> ActiveEmployees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RequestEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new EmployeeRequestEntityTypeConfiguration());

            modelBuilder.Entity<IdentityRole>().HasData(
                     new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                     new IdentityRole { Id = "2", Name = "Employee", NormalizedName = "Employee" },
                     new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" }
                 );


        }
    }
}
