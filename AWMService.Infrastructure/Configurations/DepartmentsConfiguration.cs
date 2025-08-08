using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class DepartmentsConfiguration : IEntityTypeConfiguration<Departments>
    {
        public void Configure(EntityTypeBuilder<Departments> e)
        {
            e.ToTable("Departments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(255);

            e.HasIndex(x => x.InstituteId);
            e.HasIndex(x => new { x.InstituteId, x.Name });

            e.HasOne(x => x.Institute)
                .WithMany()
                .HasForeignKey(x => x.InstituteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
