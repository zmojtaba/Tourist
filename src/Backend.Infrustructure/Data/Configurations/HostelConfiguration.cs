using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrustructure.Data.Configurations
{
    public class HostelConfiguration : IEntityTypeConfiguration<Hostel>
    {
        public void Configure(EntityTypeBuilder<Hostel> builder)
        {
            builder.ToTable("Hostels");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, v => HostelId.Of(v))
                .ValueGeneratedNever();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            // Owned Type: Address
            builder.OwnsOne(x => x.Address, a =>
            {
                a.Property(p => p.City).HasColumnName("City");
                a.Property(p => p.Region).HasColumnName("Region");
                a.Property(p => p.Country).HasColumnName("Country");
                a.Property(p => p.PostalCode).HasColumnName("PostalCode");
            });

            // Owned Type: GeoLocation
            builder.OwnsOne(x => x.AddressLocation, g =>
            {
                g.Property(p => p.Latitude).HasColumnName("Lat");
                g.Property(p => p.Longitude).HasColumnName("Lng");
            });

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
