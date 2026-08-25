using Backend.Domain.Documents;
using Backend.Domain.ValueObjects;
using Backend.Infrustructure.Data.Converter;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrustructure.Data.Configurations
{
    public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
    {
        public void Configure(EntityTypeBuilder<Verification> builder)
        {
            builder.ToTable("verifications");


            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id)
                .HasConversion(
                verificationId => verificationId.Value,
                dbId => VerificationId.Of(dbId));
                //new StronglyTypedIdConverter<VerificationId>(VerificationId.Of));

            builder.Property(v => v.AccountId)
                .HasConversion(new StronglyTypedIdConverter<AccountId>(AccountId.Of));

            builder.Property(v => v.Type).HasConversion<int>();
            builder.Property(v => v.VerificationStatus).HasConversion<int>();
            builder.Property(v => v.StatusMessage).HasMaxLength(5000);

            // 1:1 Relationship to the abstract Document
            builder.HasOne(v => v.Document)
                   .WithOne()
                   .HasForeignKey<VerificationDocument>("VerificationId") // Shadow FK
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
