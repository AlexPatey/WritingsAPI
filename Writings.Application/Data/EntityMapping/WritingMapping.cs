using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;
using Writings.Application.ValueGenerators;

namespace Writings.Application.Data.EntityMapping
{
    internal class WritingMapping : IEntityTypeConfiguration<Writing>
    {
        public void Configure(EntityTypeBuilder<Writing> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Title)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(w => w.Body)
                .HasColumnType("varchar(max)")
                .IsRequired();

            builder.Property(w => w.Type)
                .IsRequired();

            builder.Property<DateTimeOffset>("CreatedWhen")
                .HasValueGenerator<CreatedWhenDateGenerator>()
                .IsRequired();

            builder.Property<DateTimeOffset?>("LastEdited");

            builder.Property<bool>("Deleted")
                .HasDefaultValue(false);

            builder.HasQueryFilter(w => EF.Property<bool>(w, "Deleted") == false);
        }
    }
}
