using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class DefenseSchedulesConfiguration : IEntityTypeConfiguration<DefenseSchedules>
    {
        public void Configure(EntityTypeBuilder<DefenseSchedules> builder)
        {
            builder.HasKey(ds => ds.DefenseSchedulesId);
            builder.Property(ds => ds.Location)
                .HasMaxLength(500);

            builder.HasOne(ds => ds.Commissions)
                .WithMany(c => c.DefenseSchedules)
                .HasForeignKey(ds => ds.CommissionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ds => ds.StudentWorks)
                .WithMany(sw => sw.DefenseSchedules)
                .HasForeignKey(ds => ds.StudentWorkId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
