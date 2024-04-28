using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;
using Writings.Application.ValueGenerators;

namespace Writings.Application.Data.EntityMapping
{
    public class TagMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TagName)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();;

            builder.Property<DateTimeOffset>("CreatedWhen")
                .HasValueGenerator<CreatedWhenDateGenerator>()
                .IsRequired();
        }
    }
}
