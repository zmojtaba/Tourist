namespace Backend.Infrustructure.Data.Configurations
{
    public class DoorConfiguration : IEntityTypeConfiguration<Door>
    {
        public void Configure(EntityTypeBuilder<Door> builder)
        {
            builder.ToTable("Doors");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasConversion(
                    id => id.Value,
                    value => DoorId.Of(value));

            builder.Property(d => d.FacilityId)
                .HasConversion(
                    id => id.Value,
                    value => FacilityId.Of(value))
                .IsRequired();

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);


            builder.Property(b => b.Type)
                .HasConversion(
                type => type.ToString(),
                value => (DoorType)Enum.Parse(typeof(DoorType), value));

            builder.Property(b => b.Status)
                .HasConversion(
                status => status.ToString(),
                value => (DoorStatus)Enum.Parse(typeof(DoorStatus), value));

            builder.Property(d => d.RoomNumber)
                .IsRequired();

            builder.HasIndex(d => d.FacilityId);

            builder.HasIndex(d => new { d.FacilityId, d.RoomNumber }).IsUnique();


        }
    }
}
