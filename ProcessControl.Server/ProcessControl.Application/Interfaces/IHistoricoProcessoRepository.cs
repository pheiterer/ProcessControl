
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Interfaces
{
    public interface IHistoricoProcessoRepository
    {
        Task<IEnumerable<HistoricoProcesso>> GetByProcessoIdAsync(int processoId);
        Task<HistoricoProcesso> GetByIdAsync(int processoId, int id);
        Task AddAsync(HistoricoProcesso historico);
        Task UpdateAsync(HistoricoProcesso historico);
        Task DeleteAsync(int id);
    }
}
