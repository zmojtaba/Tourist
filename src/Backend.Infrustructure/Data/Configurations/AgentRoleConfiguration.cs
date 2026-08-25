using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Backend.Infrustructure.Data.Converter;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrustructure.Data.Configurations
{
    public class AgentRoleConfiguration : IEntityTypeConfiguration<AgentRole>
    {
        public void Configure(EntityTypeBuilder<AgentRole> builder)
        {
            builder.ToTable("AgentRoles");

            // ---------------------------
            // Key
            // ---------------------------
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasConversion(new StronglyTypedIdConverter<AgentRoleId>(AgentRoleId.Of))
                .ValueGeneratedNever();

            builder.Property(r => r.AccountId)
                .HasConversion(new StronglyTypedIdConverter<AccountId>(AccountId.Of))
                .IsRequired();

            //builder.Property(x => x.CurrentLocation)
            //    .HasConversion(
            //        v => v == null ? (NpgsqlTypes.NpgsqlPoint?)null : v.ToPoint(),
            //        v => v == null ? null : GeoLocation.FromPoint(v.Value))
            //    .HasColumnType("point");

            builder.OwnsOne(x => x.CurrentLocation, b =>
            {
                b.Property(p => p.Latitude).HasColumnName("Latitude");
                b.Property(p => p.Longitude).HasColumnName("Longitude");
            });

            // ---------------------------
            // Discriminator (TPH)
            // ---------------------------
            builder.HasDiscriminator<string>("role_type")
                .HasValue<DriverRole>("Driver")
                .HasValue<PassengerRole>("Passenger")
                .HasValue<TourGuideRole>("TourGuide")
                .HasValue<TranslatorRole>("Translator")
                .HasValue<HostRole>("Host");

            // Optional: index for queries
            builder.HasIndex(r => r.AccountId);
        }
    }
}
