using Backend.Domain.Documents;
using Backend.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrustructure.Data.Configurations
{

    public class VerificationDocumentConfiguration : IEntityTypeConfiguration<VerificationDocument>
    {
        public void Configure(EntityTypeBuilder<VerificationDocument> builder)
        {
            builder.ToTable("verification_documents");

            // Shadow Primary Key linked to Verification
            builder.Property<VerificationId>("VerificationId")
                .HasConversion(
                    id => id.Value,
                    value => VerificationId.Of(value));

            builder.HasKey("VerificationId");

            // TPH Discriminator
            builder.HasDiscriminator<string>("document_type")
                .HasValue<IdCardVerificationDocument>("IdCard")
                .HasValue<FaceVerificationDocument>("Face")
                .HasValue<DrivingLicenseVerificationDocument>("DrivingLicense")
                .HasValue<CarImageVerificationDocument>("CarImage")
                .HasValue<CarInsuranceVerificationDocument>("CarInsurance")
                .HasValue<AddressVerificationDocument>("Address")
                .HasValue<EnglishProficiencyVerificationDocument>("English")
                .HasValue<TourGuideVerificationDocument>("TourLeader");
        }
    }



    // Id Card
    public class IdCardDocumentConfig : IEntityTypeConfiguration<IdCardVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<IdCardVerificationDocument> builder)
        {
            builder.Property(x => x.IdNumber).HasMaxLength(50);
            builder.Property(x => x.IdCardImageUrl).IsRequired();
        }
    }

    // Face (JSONB List)
    public class FaceDocumentConfig : IEntityTypeConfiguration<FaceVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<FaceVerificationDocument> builder)
        {
            builder.Property(x => x.FaceImageUrls)
                   .HasColumnType("jsonb")
                   .HasField("_faceImageUrls")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    // Car Image (JSONB List + Properties)
    public class CarImageDocumentConfig : IEntityTypeConfiguration<CarImageVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<CarImageVerificationDocument> builder)
        {
            builder.Property(x => x.PlateNumber).HasMaxLength(20);
            builder.Property(x => x.CarImageUrls)
                   .HasColumnType("jsonb")
                   .HasField("_carImageUrls")
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    // Driving License
    public class DrivingLicenseDocumentConfig : IEntityTypeConfiguration<DrivingLicenseVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<DrivingLicenseVerificationDocument> builder)
        {
            builder.Property(x => x.LicenseNumber).HasMaxLength(50);
        }
    }

    // Address (Value Object Mapping)
    public class AddressDocumentConfig : IEntityTypeConfiguration<AddressVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<AddressVerificationDocument> builder)
        {
            // GeoLocation is likely a Value Object
            builder.OwnsOne(x => x.GeoLocation, nav =>
            {
                nav.Property(g => g.Latitude).HasColumnName("lat");
                nav.Property(g => g.Longitude).HasColumnName("lng");
            });
        }
    }

    // English Proficiency
    public class EnglishDocumentConfig : IEntityTypeConfiguration<EnglishProficiencyVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<EnglishProficiencyVerificationDocument> builder)
        {
            builder.Property(x => x.Score).HasPrecision(5, 2);
        }
    }

    // Car Insurance
    public class CarInsuranceDocumentConfig : IEntityTypeConfiguration<CarInsuranceVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<CarInsuranceVerificationDocument> builder)
        {
            builder.Property(x => x.PolicyNumber).HasMaxLength(100);
        }
    }

    // Tour Leader
    public class TourGuideDocumentConfig : IEntityTypeConfiguration<TourGuideVerificationDocument>
    {
        public void Configure(EntityTypeBuilder<TourGuideVerificationDocument> builder)
        {
            builder.Property(x => x.TourLeaderLicenseImageUrl).IsRequired();
        }
    }


}
