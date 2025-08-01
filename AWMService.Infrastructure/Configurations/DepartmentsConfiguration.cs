using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class DepartmentsConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(500);

            //public Institutes Institute { get; set; }

            builder.HasOne(d => d.Institute)
                .WithMany(i => i.Departments)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
