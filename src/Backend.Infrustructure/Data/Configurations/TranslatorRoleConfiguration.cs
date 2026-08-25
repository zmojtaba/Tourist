using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Backend.Domain.Roles;

namespace Backend.Infrustructure.Data.Configurations
{
    public class TranslatorRoleConfiguration : IEntityTypeConfiguration<TranslatorRole>
    {
        public void Configure(EntityTypeBuilder<TranslatorRole> builder)
        {
            builder.Property(x => x.IeltsPoint)
                   .HasPrecision(3, 1); // e.g. 7.5
            builder.Property("_languages")
                    .HasColumnName("Languages")
                    .HasColumnType("jsonb");
        }
    }
}
