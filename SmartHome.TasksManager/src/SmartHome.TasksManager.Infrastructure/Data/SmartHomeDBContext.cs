using Microsoft.EntityFrameworkCore;
using SmartHome.TasksManager.Heater.Entities;
using SmartHome.webapi.Entities;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace SmartHome.TasksManager.Infrastructure.Data;

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

  public virtual DbSet<UrlLog> UrlLogs { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Garage>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("Garages_pkey");

      entity.Property(e => e.Id).ValueGeneratedNever();
    });

    modelBuilder.Entity<HeatRequest>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("HeatRequests_pkey");

      entity.Property(e => e.Id).ValueGeneratedNever();

      entity.HasOne(d => d.Garage).WithMany(p => p.HeatRequests)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("garageId");
    });

    modelBuilder.Entity<HeatingLog>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("HeatingHistory_pkey");

      entity.Property(e => e.Id).ValueGeneratedNever();
    });

    modelBuilder.Entity<OutsideTemperature>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("OutsideTemperatures_pkey");

      entity.Property(e => e.Id).ValueGeneratedNever();

      entity.HasOne(d => d.Garage).WithMany(p => p.OutsideTemperatures)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("OutsideTemperatures_garageId_fkey");
    });

    modelBuilder.Entity<UrlLog>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("UrlLogs_pkey");

      entity.Property(e => e.Id).ValueGeneratedNever();
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
