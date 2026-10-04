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
                .HasConversion(
                    accountId => accountId.Value,
                    dbId => AccountId.Of(dbId)
                );


            // -----------------------------
            // Devices (Value Objects)
            // -----------------------------

            builder.HasMany(a => a.Devices).WithOne().HasForeignKey(d => d.AccountId);
        }
    }
}
