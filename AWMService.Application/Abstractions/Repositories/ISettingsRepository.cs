using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface ISettingsRepository
    {
        Task<Settings?> GetSettingsAsync(string key, CancellationToken ct);          
        Task<string?> GetValueAsync(string key, CancellationToken ct);       
        Task SetAsync(string key, string value, int actorUserId, string? description, CancellationToken ct);
    }
}
