using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartHome.webapi.Entities;

namespace SmartHome.webapi.Context;

public partial class SmartHomeDBContext : DbContext
{
    public SmartHomeDBContext()
    {
    }

    public SmartHomeDBContext(DbContextOptions<SmartHomeDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Garage> Garages { get; set; }

    public virtual DbSet<HeatRequest> HeatRequests { get; set; }

    public virtual DbSet<HeatingLog> HeatingLogs { get; set; }

    public virtual DbSet<OutsideTemperature> OutsideTemperatures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID=postgres;Password=Kf%a6C2q;Host=localhost;Port=5432;Database=SmartHomeDB;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Garage>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Garages_pkey");

            entity.ToTable("Garages", "Heater");

            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<HeatRequest>(entity =>
        {
            entity.HasKey(e => e.id).HasName("HeatRequests_pkey");

            entity.ToTable("HeatRequests", "Heater");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.heatRequest1).HasColumnName("heatRequest");

            entity.HasOne(d => d.garage).WithMany(p => p.HeatRequests)
                .HasForeignKey(d => d.garageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("garageId");
        });

        modelBuilder.Entity<HeatingLog>(entity =>
        {
            entity.HasKey(e => e.id).HasName("HeatingHistory_pkey");

            entity.ToTable("HeatingLogs", "Heater");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.date).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<OutsideTemperature>(entity =>
        {
            entity.HasKey(e => e.id).HasName("OutsideTemperatures_pkey");

            entity.ToTable("OutsideTemperatures", "Heater");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.date).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.garage).WithMany(p => p.OutsideTemperatures)
                .HasForeignKey(d => d.garageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("garageId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
