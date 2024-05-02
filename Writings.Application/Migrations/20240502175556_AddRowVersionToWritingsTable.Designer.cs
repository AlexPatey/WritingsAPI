﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Writings.Application.Data;

#nullable disable

namespace Writings.Application.Migrations
{
    [DbContext(typeof(WritingsContext))]
    [Migration("20240502175556_AddRowVersionToWritingsTable")]
    partial class AddRowVersionToWritingsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Writings.Application.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedWhen")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar");

                    b.Property<Guid>("WritingId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WritingId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Writings.Application.Models.Writing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTimeOffset>("CreatedWhen")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("LastEdited")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("YearOfCompletion")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Writings");
                });

            modelBuilder.Entity("Writings.Application.Models.Tag", b =>
                {
                    b.HasOne("Writings.Application.Models.Writing", "Writing")
                        .WithMany("Tags")
                        .HasForeignKey("WritingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Writing");
                });

            modelBuilder.Entity("Writings.Application.Models.Writing", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
