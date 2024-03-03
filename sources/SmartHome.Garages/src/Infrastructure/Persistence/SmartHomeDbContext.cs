using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Core.Common.Repositories;

namespace Infrastructure.Persistence;

public partial class SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options) : DbContext(options), IUnitOfWork
{
  private IDbContextTransaction _dbContextTransaction;
  
  public virtual DbSet<CyclicHeatTask> CyclicHeatTasks { get; set; }

    public virtual DbSet<CyclicHeatTaskDay> CyclicHeatTaskDays { get; set; }

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

            entity.ToTable("CyclicHeatTask", "Garages");

            entity.HasIndex(e => e.GarageId, "IX_CyclicHeatTask_GarageId");

            entity.HasOne(d => d.Garage).WithMany(p => p.CyclicHeatTasks)
                .HasForeignKey(d => d.GarageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });

        modelBuilder.Entity<CyclicHeatTaskDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CyclicHeatTaskDaysInWeek_pkey");

            entity.ToTable("CyclicHeatTaskDay", "Garages");

            entity.HasIndex(e => e.CyclicHeatTaskId, "IX_CyclicHeatTaskDaysInWeek_CyclicHeatTaskId");

            entity.HasIndex(e => e.Day, "IX_CyclicHeatTaskDaysInWeek_DayId");

            entity.HasOne(d => d.CyclicHeatTask).WithMany(p => p.CyclicHeatTaskDays)
                .HasForeignKey(d => d.CyclicHeatTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CyclicHeatTaskId");
        });

        modelBuilder.Entity<Garage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Garage_pkey");

            entity.ToTable("Garage", "Garages");

            entity.Property(e => e.Ip).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<HeatLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatLog_pkey");

            entity.ToTable("HeatLog", "Garages");
        });

        modelBuilder.Entity<HeatTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HeatTask_pkey");

            entity.ToTable("HeatTask", "Garages");

            entity.HasIndex(e => e.GarageId, "IX_HeatTask_GarageId");

            entity.HasOne(d => d.Garage).WithMany(p => p.HeatTasks)
                .HasForeignKey(d => d.GarageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });

        modelBuilder.Entity<OutsideTemperature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OutsideTemperature_pkey");

            entity.ToTable("OutsideTemperature", "Garages");

            entity.HasIndex(e => e.GarageId, "IX_OutsideTemperature_GarageId");

            entity.HasOne(d => d.Garage).WithMany(p => p.OutsideTemperatures)
                .HasForeignKey(d => d.GarageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageId");
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
