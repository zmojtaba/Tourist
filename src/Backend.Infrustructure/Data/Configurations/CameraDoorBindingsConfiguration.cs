using System.Text.Json;

namespace Backend.Infrustructure.Data.Configurations
{
    public class CameraDoorBindingsConfiguration : IEntityTypeConfiguration<CameraDoorBinding>
    {
        public void Configure(EntityTypeBuilder<CameraDoorBinding> builder)
        {
            builder.HasKey(cdb => cdb.Id);
            builder.Property(cdb => cdb.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => CameraDoorBindingId.Of(dbId)
                );

            builder.Property(b => b.CameraId)
                .HasConversion(
                    id => id.Value,
                    value => CameraId.Of(value))
                .IsRequired();

            builder.Property(b => b.FacilityId)
                .HasConversion(
                    id => id.Value,
                    value => FacilityId.Of(value))
                .IsRequired();

            builder.Property(b => b.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(b => b.Type)
                .HasConversion(
                type => type.ToString(),
                value => (CameraDoorBindingType)Enum.Parse( typeof(CameraDoorBindingType), value ) );


            builder.Property(b => b.DoorIds)
                .HasField("_doorIds")
                .HasConversion(
                    doorIds => JsonSerializer.Serialize(
                        doorIds.Select(x => x.Value).ToList(),
                        (JsonSerializerOptions?)null),

                    json => JsonSerializer.Deserialize<List<Guid>>(
                        json,
                        (JsonSerializerOptions?)null)!
                        .Select(DoorId.Of)
                        .ToList());


            builder.HasIndex(b => b.CameraId);
            builder.HasIndex(b => b.FacilityId);


        }
    }
}
