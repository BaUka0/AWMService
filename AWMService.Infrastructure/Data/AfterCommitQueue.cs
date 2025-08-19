using AWMService.Application.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Data
{
    public sealed class AfterCommitQueue : IAfterCommitQueue
    {
        private readonly List<Func<CancellationToken, Task>> _actions = new();

        public void Enqueue(Func<CancellationToken, Task> action) => _actions.Add(action);

        public async Task FlushAsync(bool committed, CancellationToken ct)
        {
            var actions = _actions.ToArray();
            _actions.Clear();
            if (!committed) return;
            foreach (var a in actions) await a(ct);
        }
    }
}
