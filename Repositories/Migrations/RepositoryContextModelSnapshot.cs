﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Repositories;

#nullable disable

namespace Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Models.Amenity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Amenity_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Amenities");
                });

            modelBuilder.Entity("Entities.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Address = "Street 102, Building 3, House 11",
                            Email = "john_doe@test.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Password = "$2a$11$QwvfxSYtIiN1Gb3IfAewOOCwtVis2.69j4PFeCg4b.rBavXIASR1G",
                            PhoneNumber = "462225252"
                        });
                });

            modelBuilder.Entity("Entities.Models.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ReservationStatus")
                        .HasColumnType("integer");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Entities.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Floor")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price_Per_Night")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Entities.Models.RoomAmenity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AmenityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AmenityId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAmenities");
                });

            modelBuilder.Entity("Entities.Models.RoomAmenity", b =>
                {
                    b.HasOne("Entities.Models.Amenity", "Amenity")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Models.Room", "Room")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenity");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Entities.Models.Amenity", b =>
                {
                    b.Navigation("RoomAmenities");
                });

            modelBuilder.Entity("Entities.Models.Room", b =>
                {
                    b.Navigation("RoomAmenities");
                });
#pragma warning restore 612, 618
        }
    }
}
