using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrustructure.Data.Configurations
{

    public class HostRoleConfiguration : IEntityTypeConfiguration<HostRole>
    {
        public void Configure(EntityTypeBuilder<HostRole> builder)
        {

            builder.HasMany(x => x.Hostels)
                    .WithOne()
                    .HasForeignKey("AgentRoleId") // Same FK column name as Driver
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Hostels)
                .HasField("_hostels")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }

}
