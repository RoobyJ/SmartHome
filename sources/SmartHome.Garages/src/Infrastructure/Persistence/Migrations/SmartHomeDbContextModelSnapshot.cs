﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartHome.Infrastructure.Data;

#nullable disable

namespace SmartHome.Infrastructure.Migrations
{
    [DbContext(typeof(SmartHomeDbContext))]
    partial class SmartHomeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SmartHome.Core.Entities.CyclicHeatTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GarageId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("interval");

                    b.HasKey("Id")
                        .HasName("CyclicHeatTask_pkey");

                    b.HasIndex("GarageId");

                    b.ToTable("CyclicHeatTask", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.CyclicHeatTaskDaysInWeek", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CyclicHeatTaskId")
                        .HasColumnType("integer");

                    b.Property<int>("DayId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("CyclicHeatTaskDaysInWeek_pkey");

                    b.HasIndex("CyclicHeatTaskId");

                    b.HasIndex("DayId");

                    b.ToTable("CyclicHeatTaskDaysInWeek", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.DayInWeek", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("DayInWeek_pkey");

                    b.ToTable("DayInWeek", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Garage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("Garage_pkey");

                    b.ToTable("Garage", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.HeatLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Info")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("HeatLog_pkey");

                    b.ToTable("HeatLog", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.HeatTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GarageId")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("HeatTask_pkey");

                    b.HasIndex("GarageId");

                    b.ToTable("HeatTask", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.OutsideTemperature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GarageId")
                        .HasColumnType("integer");

                    b.Property<int>("Temperature")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("OutsideTemperature_pkey");

                    b.HasIndex("GarageId");

                    b.ToTable("OutsideTemperature", "Garages");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.CyclicHeatTask", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Garage", "Garage")
                        .WithMany("CyclicHeatTasks")
                        .HasForeignKey("GarageId")
                        .IsRequired()
                        .HasConstraintName("GarageId");

                    b.Navigation("Garage");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.CyclicHeatTaskDaysInWeek", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.CyclicHeatTask", "CyclicHeatTask")
                        .WithMany("CyclicHeatTaskDaysInWeeks")
                        .HasForeignKey("CyclicHeatTaskId")
                        .IsRequired()
                        .HasConstraintName("CyclicHeatTaskId");

                    b.HasOne("SmartHome.Core.Entities.DayInWeek", "Day")
                        .WithMany("CyclicHeatTaskDaysInWeeks")
                        .HasForeignKey("DayId")
                        .IsRequired()
                        .HasConstraintName("DayId");

                    b.Navigation("CyclicHeatTask");

                    b.Navigation("Day");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.HeatTask", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Garage", "Garage")
                        .WithMany("HeatTasks")
                        .HasForeignKey("GarageId")
                        .IsRequired()
                        .HasConstraintName("GarageId");

                    b.Navigation("Garage");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.OutsideTemperature", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Garage", "Garage")
                        .WithMany("OutsideTemperatures")
                        .HasForeignKey("GarageId")
                        .IsRequired()
                        .HasConstraintName("GarageId");

                    b.Navigation("Garage");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.CyclicHeatTask", b =>
                {
                    b.Navigation("CyclicHeatTaskDaysInWeeks");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.DayInWeek", b =>
                {
                    b.Navigation("CyclicHeatTaskDaysInWeeks");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Garage", b =>
                {
                    b.Navigation("CyclicHeatTasks");

                    b.Navigation("HeatTasks");

                    b.Navigation("OutsideTemperatures");
                });
#pragma warning restore 612, 618
        }
    }
}
