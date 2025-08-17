using AWMService.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace AWMService.Infrastructure.Data
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _tx;

        public UnitOfWork(AppDbContext ctx, ILogger<UnitOfWork> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_tx is not null) throw new InvalidOperationException("Transaction already started.");
            _tx = await _ctx.Database.BeginTransactionAsync(ct);
            _logger.LogInformation("Beginning new database transaction {TransactionId}", _tx.TransactionId);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_tx is null) throw new InvalidOperationException("No active transaction to commit.");
            try
            {
                await _ctx.SaveChangesAsync(ct);
                await _tx.CommitAsync(ct);
                _logger.LogInformation("Committed database transaction {TransactionId}", _tx.TransactionId);
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
                _logger.LogWarning("Rolled back database transaction {TransactionId}", _tx.TransactionId);
            }
            finally
            {
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public ValueTask DisposeAsync()
        {
            if (_tx != null)
            {
                _logger.LogDebug("Disposing active transaction {TransactionId}", _tx.TransactionId);
                return _tx.DisposeAsync();
            }
            return ValueTask.CompletedTask;
        }
    }
}
