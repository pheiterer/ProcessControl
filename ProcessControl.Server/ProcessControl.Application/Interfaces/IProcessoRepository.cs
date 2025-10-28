
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Interfaces
{
    public interface IProcessoRepository
    {
        Task<IEnumerable<Processo>> GetAllAsync();
        Task<IEnumerable<Processo>> GetByNumeroProcessoAsync(string numeroProcesso);
        Task<Processo> GetByIdAsync(int id);
        Task AddAsync(Processo processo);
        Task UpdateAsync(Processo processo);
        Task DeleteAsync(int id);
    }
}
