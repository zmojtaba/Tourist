namespace Backend.Infrustructure.Data.Configurations
{
    public class CameraConfiguration : IEntityTypeConfiguration<Camera>
    {
        public void Configure(EntityTypeBuilder<Camera> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                    id => id.Value,
                    value => CameraId.Of(value));


            builder.Property(c => c.FacilityId)
                .HasConversion(
                    id => id.Value,
                    value => FacilityId.Of(value))
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Url)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(b => b.CameraSourceType)
                .HasConversion(
                type => type.ToString(),
                value => (CameraSourceType)Enum.Parse(typeof(CameraSourceType), value));

            builder.Property(b => b.Type)
                .HasConversion(
                type => type.ToString(),
                value => (CameraType)Enum.Parse(typeof(CameraType), value));

            builder.Property(b => b.Status)
                .HasConversion(
                type => type.ToString(),
                value => (CameraStatus)Enum.Parse(typeof(CameraStatus), value));

            builder.HasIndex(c => c.FacilityId);

            builder.HasIndex(c => new { c.FacilityId, c.Name, c.Url }).IsUnique();


        }
    }
}
