using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;

namespace WFH.infrastructure
{
    public class RequestEntityTypeConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Request")
            .HasDiscriminator<string>("RequestType")
            .HasValue<Request>("Request")
            .HasValue<WorkFromHomeRequest>("WorkFromHomeRequest");


            builder.Property(e => e.RequestStatus)
                 .HasConversion(
                RequestStatusEnums => RequestStatusEnums.Id,
                id => RequestStatusEnums.FromId(id)
                );



            builder.HasKey(r => r.Id);


            builder.Property(r => r.RequestFrom).IsRequired();
            builder.Property(r => r.RequestUpTo).IsRequired();
            builder.Property(r => r.ApproverIdentifier).IsRequired();
            builder.Property(r => r.RequestStatus).IsRequired();

            builder.HasOne(r => r.approval).WithOne(a => a.Request).HasForeignKey<Approval>(a => a.RequestId);

            builder.HasOne(r => r.Employee).WithMany(e => e.Requests).HasForeignKey(x => x.RequestedBy);

        }
    }
}
