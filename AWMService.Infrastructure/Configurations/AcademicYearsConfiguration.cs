using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class AcademicYearsConfiguration : IEntityTypeConfiguration<AcademicYears>
    {


        public void Configure(EntityTypeBuilder<AcademicYears> builder)
        {
            builder.HasKey(ay => ay.AcademicYearId);
            builder.Property(ay => ay.YearName)
                .HasMaxLength(50);
        }
    }
 
}
