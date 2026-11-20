
namespace Backend.Infrustructure.Data.Configurations
{
    public class CameraDoorBindingDoorConfiguration : IEntityTypeConfiguration<CameraDoorBindingDoor>
    {
        public void Configure(EntityTypeBuilder<CameraDoorBindingDoor> builder)
        {
            builder.ToTable("CameraDoorBindingDoors");

            builder.Property(x => x.CameraDoorBindingId)
                .HasConversion(
                    id => id.Value,
                    value => CameraDoorBindingId.Of(value));

            builder.Property(x => x.DoorId)
                .HasConversion(
                    id => id.Value,
                    value => DoorId.Of(value));

            builder.HasKey(x => new
            {
                x.CameraDoorBindingId,
                x.DoorId
            });
            builder.HasOne<CameraDoorBinding>()
                .WithMany()
                .HasForeignKey(x => x.CameraDoorBindingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Door>()
                .WithMany()
                .HasForeignKey(x => x.DoorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.DoorId);

        }
    }
}
