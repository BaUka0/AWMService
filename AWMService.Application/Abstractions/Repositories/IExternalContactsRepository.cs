using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IExternalContactsRepository
    {
        Task<ExternalContacts?> GetContactstByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<ExternalContacts>> ListAllAsync(CancellationToken ct);

        Task AddContactsAsync(string fullName, string? position, string? organization, string? email, string? phoneNumber, int actorUserId, CancellationToken ct);
        Task UpdateContactsAsync(int id, string? fullName, string? position, string? organization, string? email, string? phoneNumber, int actorUserId, CancellationToken ct);
        Task SoftDeleteContactsAsync(int id, int actorUserId, CancellationToken ct);
    }
}
