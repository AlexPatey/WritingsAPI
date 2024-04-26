﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;

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

            builder.Property(w => w.YearOfCompletion);

            builder.Property(w => w.UploadedWhen)
                .IsRequired();

            builder.Property(w => w.LastEdited)
                .IsRequired();
        }
    }
}
