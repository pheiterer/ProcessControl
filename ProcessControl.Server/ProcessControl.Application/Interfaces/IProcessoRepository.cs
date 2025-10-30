
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Interfaces
{
    public interface IProcessoRepository
    {
        Task<IEnumerable<Processo>> GetProcessListAsync(int page, int limit, string? searchTerm);
        Task<Processo?> GetByIdAsync(int id);
        Task AddAsync(Processo processo);
        Task UpdateAsync(Processo processo);
        Task DeleteAsync(int id);
    }
}
