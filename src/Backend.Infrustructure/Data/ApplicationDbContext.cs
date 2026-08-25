using Backend.Domain.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Backend.Infrustructure.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account>  Accounts => Set<Account>();
        public DbSet<AgentRole> AgentRoles => Set<AgentRole>();
        public DbSet<Verification> Verifications => Set<Verification>();

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Hostel> Hostels => Set<Hostel>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //ConfigureAccountIdentityLink(builder);  
            base.OnModelCreating(builder);

        }

        //private void ConfigureAccountIdentityLink(ModelBuilder builder)
        //{
        //    // If your UserId (Strongly Typed ID) wraps a Guid, 
        //    // and your ApplicationUser uses the default string ID:
        //    builder.Entity<Account>()
        //        .HasOne<ApplicationUser>()
        //        .WithOne()
        //        .HasForeignKey<Account>(a => a.UserId)
        //        .IsRequired(false) // Account might not have a user yet
        //        .OnDelete(DeleteBehavior.Cascade);
        //}
    }

}
