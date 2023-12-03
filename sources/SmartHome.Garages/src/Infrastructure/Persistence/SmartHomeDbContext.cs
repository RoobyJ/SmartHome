using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Entities;
using SmartHome.Infrastructure.Persistence.Initializers;

namespace SmartHome.Infrastructure.Persistence;

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


  public virtual DbSet<CyclicHeatTask> CyclicHeatTasks { get; set; }

  public virtual DbSet<CyclicHeatTaskDaysInWeek> CyclicHeatTaskDaysInWeeks { get; set; }

  public virtual DbSet<DayInWeek> DayInWeeks { get; set; }

  public virtual DbSet<Garage> Garages { get; set; }

  public virtual DbSet<HeatLog> HeatLogs { get; set; }

  public virtual DbSet<HeatTask> HeatTasks { get; set; }

  public virtual DbSet<OutsideTemperature> OutsideTemperatures { get; set; }
  

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
        modelBuilder.Entity<CyclicHeatTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CyclicHeatTask_pkey");

            entity.HasOne(d => d.Garage).WithMany(p => p.CyclicHeatTasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });

        modelBuilder.Entity<CyclicHeatTaskDaysInWeek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CyclicHeatTaskDaysInWeek_pkey");

            entity.HasOne(d => d.CyclicHeatTask).WithMany(p => p.CyclicHeatTaskDaysInWeeks)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CyclicHeatTaskId");

            entity.HasOne(d => d.Day).WithMany(p => p.CyclicHeatTaskDaysInWeeks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DayId");
        });

        modelBuilder.Entity<DayInWeek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DayInWeek_pkey");
        });

        modelBuilder.Entity<Garage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Garage_pkey");
        });

        modelBuilder.Entity<HeatLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatLog_pkey");
        });

        modelBuilder.Entity<HeatTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatTask_pkey");

            entity.HasOne(d => d.Garage).WithMany(p => p.HeatTasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });

        modelBuilder.Entity<OutsideTemperature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OutsideTemperature_pkey");

            entity.HasOne(d => d.Garage).WithMany(p => p.OutsideTemperatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });
        modelBuilder.SeedWithStaticData();
        OnModelCreatingPartial(modelBuilder);
    }

   partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
