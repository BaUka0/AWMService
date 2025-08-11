using AWMService.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace AWMService.Infrastructure.Data
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly AppDbContext _ctx;
        private IDbContextTransaction? _tx;

        public UnitOfWork(AppDbContext ctx) => _ctx = ctx;

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_tx is not null) throw new InvalidOperationException("Transaction already started.");
            _tx = await _ctx.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_tx is null) throw new InvalidOperationException("No active transaction to commit.");
            try
            {
                await _ctx.SaveChangesAsync(ct);
                await _tx.CommitAsync(ct);
            }
            finally
            {
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_tx is null) return;
            try
            {
                await _tx.RollbackAsync(ct);
            }
            finally
            {
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _ctx.SaveChangesAsync(ct);

        public ValueTask DisposeAsync()
            => _tx?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}