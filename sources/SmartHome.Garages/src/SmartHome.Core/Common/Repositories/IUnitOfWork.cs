using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Common.Repositories;

using System.Data;

public interface IUnitOfWork
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

  Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
    CancellationToken cancellationToken = default);

  Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}
