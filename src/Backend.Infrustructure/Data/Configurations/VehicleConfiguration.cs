using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrustructure.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, v => VehicleId.Of(v))
                .ValueGeneratedNever();

            builder.Property<AgentRoleId>("AgentRoleId")
                .HasConversion(id => id.Value, v => AgentRoleId.Of(v));

            builder.Property(v => v.Model).IsRequired();
            builder.Property(v => v.Color).HasMaxLength(50);

            // List of strings (Images)
            builder.Property(x => x.Images)
                .HasField("_images")
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        }
    }
}
