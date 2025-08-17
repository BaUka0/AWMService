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
    public class ExternalContactsRepository : IExternalContactsRepository
    {
        private readonly AppDbContext _context;
        public ExternalContactsRepository(AppDbContext context)=> _context = context;

        public async Task<ExternalContacts?> GetContactstByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<ExternalContacts>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        }

        public async Task<IReadOnlyList<ExternalContacts>> ListAllAsync(CancellationToken ct)
        {
           return  await _context.Set<ExternalContacts>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task AddContactsAsync(string fullName, string? position, string? organization, string? email, string? phoneNumber, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.", nameof(fullName));

            var now = DateTime.UtcNow;

            var entity = new ExternalContacts
            {
                FullName = fullName.Trim(),
                Position = string.IsNullOrWhiteSpace(position) ? null : position.Trim(),
                Organization = string.IsNullOrWhiteSpace(organization) ? null : organization.Trim(),
                Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim(),
                CreatedBy = actorUserId,
                CreatedOn = now,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };
            await _context.Set<ExternalContacts>().AddAsync(entity, ct);
        }


        public async Task UpdateContactsAsync(int id, string? fullName, string? position, string? organization, string? email, string? phoneNumber, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<ExternalContacts>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"ExternalContact #{id} not found.");

            if (fullName is not null) entity.FullName = fullName.Trim();
            if (position is not null) entity.Position = position.Trim();
            if (organization is not null) entity.Organization = organization.Trim();
            if (email is not null) entity.Email = email.Trim();
            if (phoneNumber is not null) entity.PhoneNumber = phoneNumber.Trim();

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


        public async Task SoftDeleteContactsAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<ExternalContacts>()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }



}
