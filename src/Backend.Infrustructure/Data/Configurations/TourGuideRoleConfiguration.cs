using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backend.Domain.Roles;

namespace Backend.Infrustructure.Data.Configurations
{
    public class TourGuideRoleConfiguration : IEntityTypeConfiguration<TourGuideRole>
    {
        public void Configure(EntityTypeBuilder<TourGuideRole> builder)
        {
            builder.Property<string>("Message");
        }
    }
}
