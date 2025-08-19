using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDbContext _context;
        public SettingsRepository(AppDbContext context) => _context = context;

        public Task<Settings?> GetSettingsAsync(string key, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(key)) return Task.FromResult<Settings?>(null);
            var k = key.Trim();
            return _context.Set<Settings>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SettingKey == k, ct);
        }

        public async Task<string?> GetValueAsync(string key, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            var k = key.Trim();

            var val = await _context.Set<Settings>()
                .AsNoTracking()
                .Where(s => s.SettingKey == k)
                .Select(s => s.SettingValue)
                .FirstOrDefaultAsync(ct);

            return val; 
        }

        public async Task SetAsync(string key, string value, int actorUserId, string? description, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key is required.", nameof(key));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            var k = key.Trim();
            var entity = await _context.Set<Settings>()
                .FirstOrDefaultAsync(s => s.SettingKey == k, ct);

            var now = DateTime.UtcNow;

            if (entity is null)
            {
                await _context.Set<Settings>().AddAsync(new Settings
                {
                    SettingKey = k,
                    SettingValue = value, 
                    Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                    CreatedOn = now,
                    CreatedBy = actorUserId,
                    ModifiedBy = null,
                    ModifiedOn = null
                }, ct);

                return;
            }

            entity.SettingValue = value;
            if (description is not null)
                entity.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = now;
        }
    }
}
