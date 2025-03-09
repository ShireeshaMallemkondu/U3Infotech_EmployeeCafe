﻿// <auto-generated />
using System;
using FullStackAssessment_U3Infotech.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FullStackAssessment_U3Infotech.Migrations
{
    [DbContext(typeof(AssessmentDbContext))]
    partial class AssessmentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FullStackAssessment_U3Infotech.Models.Cafe", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Cafes");
                });

            modelBuilder.Entity("FullStackAssessment_U3Infotech.Models.Employee", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email_Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phonenumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("FullStackAssessment_U3Infotech.Models.EmployeeCafeRelation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CafeID")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("CafeID");

                    b.HasIndex("EmployeeID")
                        .IsUnique();

                    b.ToTable("EmployeeCafeRelations");
                });

            modelBuilder.Entity("FullStackAssessment_U3Infotech.Models.EmployeeCafeRelation", b =>
                {
                    b.HasOne("FullStackAssessment_U3Infotech.Models.Cafe", "Cafe")
                        .WithMany("EmployeeCafeRelations")
                        .HasForeignKey("CafeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FullStackAssessment_U3Infotech.Models.Employee", "Employee")
                        .WithMany("EmployeeCafeRelations")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cafe");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("FullStackAssessment_U3Infotech.Models.Cafe", b =>
                {
                    b.Navigation("EmployeeCafeRelations");
                });

            modelBuilder.Entity("FullStackAssessment_U3Infotech.Models.Employee", b =>
                {
                    b.Navigation("EmployeeCafeRelations");
                });
#pragma warning restore 612, 618
        }
    }
}
