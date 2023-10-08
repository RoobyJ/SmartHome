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


  public virtual DbSet<CyclicHeatTask> CyclicHeatTasks { get; set; }

  public virtual DbSet<CyclicHeatTaskDaysInWeek> CyclicHeatTaskDaysInWeeks { get; set; }

  public virtual DbSet<DaysInWeek> DaysInWeeks { get; set; }

  public virtual DbSet<Garage> Garages { get; set; }

  public virtual DbSet<HeatTask> HeatTasks { get; set; }

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
        modelBuilder.Entity<CyclicHeatTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CyclicHeatRequests_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Garages\".\"CyclicHeatRequests_Id_seq\"'::regclass)");

            entity.HasOne(d => d.Garage).WithMany(p => p.CyclicHeatTasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });

        modelBuilder.Entity<CyclicHeatTaskDaysInWeek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CyclicHeatTask_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Garages\".\"CyclicHeatTask_Id_seq\"'::regclass)");

            entity.HasOne(d => d.CyclicHeatTaskTime).WithMany(p => p.CyclicHeatTaskDaysInWeeks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CyclicHeatTaskId");

            entity.HasOne(d => d.Day).WithMany(p => p.CyclicHeatTaskDaysInWeeks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DayId");
        });

        modelBuilder.Entity<DaysInWeek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DayInWeek_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Garages\".\"DayInWeek_Id_seq\"'::regclass)");
        });

        modelBuilder.Entity<Garage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Garages_pkey");
        });

        modelBuilder.Entity<HeatTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatRequests_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Garages\".\"HeatRequests_Id_seq\"'::regclass)");

            entity.HasOne(d => d.Garage).WithMany(p => p.HeatTasks)
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
