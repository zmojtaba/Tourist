namespace Backend.Infrustructure.Data.Configurations
{
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id).HasConversion(
                    facilityId => facilityId.Value,
                    dbId => FacilityId.Of(dbId)
                );

            builder.Property(f => f.Name).IsRequired();

            builder.Property(f => f.Status).HasDefaultValue(FacilityStatus.Active).HasConversion(
                status => status.ToString(),
                statusDb => (FacilityStatus)Enum.Parse(typeof(FacilityStatus), statusDb)
                );

            builder.Property(f => f.Type).HasDefaultValue(FacilityType.Home).HasConversion(
                    type => type.ToString(),
                    typeDb => (FacilityType)Enum.Parse(typeof(FacilityType), typeDb)
                );






        }
    }
}
