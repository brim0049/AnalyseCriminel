﻿// <auto-generated />
using System;
using AnalyseApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AnalyseApi.Migrations
{
    [DbContext(typeof(AnalyseDbContext))]
    [Migration("20230309203711_finall")]
    partial class finall
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AnalyseApi.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NameStreet")
                        .HasColumnType("longtext");

                    b.Property<int?>("NoStreet")
                        .HasColumnType("int");

                    b.Property<string>("Ville")
                        .HasColumnType("longtext");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("AnalyseApi.Models.Call", b =>
                {
                    b.Property<int>("CallId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Number")
                        .HasColumnType("longtext");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Zone")
                        .HasColumnType("longtext");

                    b.HasKey("CallId");

                    b.HasIndex("PersonId");

                    b.ToTable("Calls");
                });

            modelBuilder.Entity("AnalyseApi.Models.Car", b =>
                {
                    b.Property<int>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Mark")
                        .HasColumnType("longtext");

                    b.Property<string>("Matricule")
                        .HasColumnType("longtext");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("CarId");

                    b.HasIndex("PersonId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("AnalyseApi.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("EventId");

                    b.HasIndex("AddressId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AnalyseApi.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("CriminalRecord")
                        .HasColumnType("int");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("NIN")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.HasKey("PersonId");

                    b.HasIndex("AddressId");

                    b.HasIndex("EventId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("AnalyseApi.Models.Relationship", b =>
                {
                    b.Property<int>("RelationshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("Relation")
                        .HasColumnType("int");

                    b.Property<int?>("SuspectId")
                        .HasColumnType("int");

                    b.HasKey("RelationshipId");

                    b.HasIndex("PersonId");

                    b.ToTable("Relationship");
                });

            modelBuilder.Entity("AnalyseApi.Models.Call", b =>
                {
                    b.HasOne("AnalyseApi.Models.Person", null)
                        .WithMany("Calls")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("AnalyseApi.Models.Car", b =>
                {
                    b.HasOne("AnalyseApi.Models.Person", null)
                        .WithMany("Cars")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("AnalyseApi.Models.Event", b =>
                {
                    b.HasOne("AnalyseApi.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("AnalyseApi.Models.Person", b =>
                {
                    b.HasOne("AnalyseApi.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("AnalyseApi.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Address");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("AnalyseApi.Models.Relationship", b =>
                {
                    b.HasOne("AnalyseApi.Models.Person", "Person")
                        .WithMany("Relations")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AnalyseApi.Models.Person", b =>
                {
                    b.Navigation("Calls");

                    b.Navigation("Cars");

                    b.Navigation("Relations");
                });
#pragma warning restore 612, 618
        }
    }
}
