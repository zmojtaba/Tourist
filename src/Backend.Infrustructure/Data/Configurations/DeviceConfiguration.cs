using Backend.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrustructure.Data.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                    deviceId => deviceId.Value,
                    dbId => DeviceId.Of(dbId)
                );


            builder.Property<string>("Name").IsRequired();
            builder.Property<string>("OperatingSystem").IsRequired();
            builder.Property<string>("Ip").IsRequired();
            builder.Property<string>("Location").IsRequired();

        }
    }
}
