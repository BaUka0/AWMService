using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Data
{
    public interface IAfterCommitQueue
    {
        void Enqueue(Func<CancellationToken, Task> action);
        Task FlushAsync(bool committed, CancellationToken ct);
    }
}
