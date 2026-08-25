using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backend.Domain.ValueObjects;
using Backend.Infrustructure.Data.Converter;

namespace Backend.Infrustructure.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            // -----------------------------
            // Id
            // -----------------------------
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(new StronglyTypedIdConverter<AccountId>(AccountId.Of))
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .HasConversion(new StronglyTypedIdConverter<UserId>(UserId.Of));

            // -----------------------------
            // Devices (Value Objects)
            // -----------------------------

            builder.Ignore(x => x.Devices);
            builder.OwnsMany<Device>("_devices", b =>
            {
                b.ToTable("AccountDevices");

                b.WithOwner().HasForeignKey("AccountId");

                b.Property<int>("Id");
                b.HasKey("Id");

                b.Property<string>("Name").IsRequired();
                b.Property<string>("OperatingSystem").IsRequired();
                b.Property<string>("Ip").IsRequired();
                b.Property<string>("Location").IsRequired();
            });

            // -----------------------------
            // Roles (One-to-Many)
            // -----------------------------
            //builder.Ignore(x => x.Roles);

            // -----------------------------
            // Roles (backing field)
            // -----------------------------
            //builder.HasMany<AgentRole>("_roles")
            //    .WithOne()
            //    .HasForeignKey(r => r.AccountId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
