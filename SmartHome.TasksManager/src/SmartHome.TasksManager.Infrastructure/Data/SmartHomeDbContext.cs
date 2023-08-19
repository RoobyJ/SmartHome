using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartHome.TasksManager.Core.Entities;
using SmartHome.TasksManager.Heater.Entities;

namespace SmartHome.TasksManager.Infrastructure.Data;

public partial class SmartHomeDbContext : DbContext
{
    public SmartHomeDbContext()
    {
    }

    public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Garage> Garages { get; set; }

    public virtual DbSet<HeatRequest> HeatRequests { get; set; }

    public virtual DbSet<HeatingLog> HeatingLogs { get; set; }

    public virtual DbSet<OutsideTemperature> OutsideTemperatures { get; set; }

    public virtual DbSet<UrlLog> UrlLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Garage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Garages_pkey");
        });

        modelBuilder.Entity<HeatRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatRequests_pkey");

            entity.HasOne(d => d.Garage).WithMany(p => p.HeatRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("garageId");
        });

        modelBuilder.Entity<HeatingLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatingLogs_pkey");
        });

        modelBuilder.Entity<OutsideTemperature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OutsideTemperatures_pkey");

            entity.HasOne(d => d.Garage).WithMany(p => p.OutsideTemperatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });

        modelBuilder.Entity<UrlLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UrlLogs_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
