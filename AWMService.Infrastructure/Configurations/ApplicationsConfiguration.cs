using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class ApplicationsConfiguration : IEntityTypeConfiguration<Applications>
    {
        public void Configure(EntityTypeBuilder<Applications> builder)
        {
            builder.HasKey(c => c.ApplicationId);
            builder.HasOne(c => c.Student)
                .WithMany(c=>c.Applications)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Topic)
                .WithMany(c=>c.Applications)
                .HasForeignKey(c => c.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Status)
                .WithMany(c => c.Applications)
                .HasForeignKey(c => c.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
