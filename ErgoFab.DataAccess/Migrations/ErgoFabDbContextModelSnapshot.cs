﻿// <auto-generated />
using System;
using ErgoFab.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ErgoFab.DataAccess.Migrations
{
    [DbContext(typeof(ErgoFabDbContext))]
    partial class ErgoFabDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SampleDb.Entities.Country", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<short>("Id"));

                    b.Property<string>("EnglishName")
                        .HasColumnType("text");

                    b.Property<byte[]>("Flag")
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("SampleDb.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<short?>("CountryId")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("SampleDb.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<short?>("CountryId")
                        .HasColumnType("smallint");

                    b.Property<int?>("DirectorId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("DirectorId");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("SampleDb.Entities.Employee", b =>
                {
                    b.HasOne("SampleDb.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("SampleDb.Entities.Organization", "Organization")
                        .WithMany("Employees")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("SampleDb.Entities.Organization", b =>
                {
                    b.HasOne("SampleDb.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("SampleDb.Entities.Employee", "Director")
                        .WithMany()
                        .HasForeignKey("DirectorId");

                    b.Navigation("Country");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("SampleDb.Entities.Organization", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}