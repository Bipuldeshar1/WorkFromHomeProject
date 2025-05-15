using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;

namespace WFH.infrastructure
{
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees")
           .HasDiscriminator<string>("EmployeeType")
          .HasValue<Employee>("Employee")
          .HasValue<Manager>("Manager");


            builder.HasKey(e => e.Id);



            builder.Property(e => e.Role)
                 .HasConversion(
                role => role.Id,
                id => RoleEnums.FromId(id)
                );


            builder.HasQueryFilter(e => !e.IsDeleted);


            builder.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(50).IsRequired();
            builder.Property(e => e.PhoneNumber).HasMaxLength(10).IsRequired();
            builder.Property(e => e.Role).HasMaxLength(50).IsRequired();

            builder.HasOne(e => e.AppUser).WithOne().HasForeignKey<Employee>(e => e.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
