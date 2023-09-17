using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Data;

internal partial class SmartHomeDbContext : DbContext, IUnitOfWork
{
  private IDbContextTransaction _dbContextTransaction;

  public SmartHomeDbContext()
  {
  }

  public SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options)
    : base(options)
  {
  }


  public virtual DbSet<CyclicHeatRequest> CyclicHeatRequests { get; set; }

  public virtual DbSet<Garage> Garages { get; set; }

  public virtual DbSet<HeatRequest> HeatRequests { get; set; }

  public virtual DbSet<HeatingLog> HeatingLogs { get; set; }

  public virtual DbSet<OutsideTemperature> OutsideTemperatures { get; set; }

  public virtual DbSet<UrlLog> UrlLogs { get; set; }

  public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
    CancellationToken cancellationToken = default)
  {
    _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    return _dbContextTransaction;
  }

  public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
  {
    if (_dbContextTransaction == null)
    {
      return;
    }

    await _dbContextTransaction.CommitAsync(cancellationToken);
  }

  public virtual Task<int> SaveChangesAsync()
  {
    return SaveChangesAsync(new CancellationToken());
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<CyclicHeatRequest>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("CyclicHeatRequests_pkey");

      entity.HasOne(d => d.Garage).WithMany(p => p.CyclicHeatRequests)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("GarageId");
    });

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
