namespace Backend.Infrustructure.Data.Configurations
{
    public class CameraDoorBindingsConfiguration : IEntityTypeConfiguration<CameraDoorBinding>
    {
        public void Configure(EntityTypeBuilder<CameraDoorBinding> builder)
        {
            builder.ToTable("CameraDoorBindings");
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

            builder.HasOne<Facility>().WithMany()
                .HasForeignKey(cdb => cdb.FacilityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Camera>().WithMany()
                .HasForeignKey(cdb => cdb.CameraId)
                .OnDelete(DeleteBehavior.NoAction);



            builder.HasIndex(b => b.CameraId);
            builder.HasIndex(b => b.FacilityId);


        }
    }
}
