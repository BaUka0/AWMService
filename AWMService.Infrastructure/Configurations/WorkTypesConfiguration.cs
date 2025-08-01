using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class WorkTypesConfiguration : IEntityTypeConfiguration<WorkTypes>
    {
        public void Configure(EntityTypeBuilder<WorkTypes> builder)
        {
            builder.HasKey(wt => wt.WorkTypeId);
            builder.Property(wt => wt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
