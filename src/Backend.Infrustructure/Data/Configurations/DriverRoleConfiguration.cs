using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backend.Domain.Roles;
using Backend.Infrustructure.Data.Converter;

namespace Backend.Infrustructure.Data.Configurations
{
    public class DriverRoleConfiguration : IEntityTypeConfiguration<DriverRole>
    {
        public void Configure(EntityTypeBuilder<DriverRole> builder)
        {
            //builder.ToTable("DriverRoles");
            builder.HasMany(x => x.Vehicles)
                    .WithOne()
                    .HasForeignKey("AgentRoleId") // This shadow FK will be created in the Vehicles table
                    .OnDelete(DeleteBehavior.Cascade);

            // Ensure EF knows to use the field for the collection
            builder.Navigation(x => x.Vehicles)
                .HasField("_vehicles")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }

}
