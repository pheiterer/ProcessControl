
using ProcessControl.Application.DTOs;

namespace ProcessControl.Application.Interfaces
{
    public interface IHistoricoProcessoService
    {
        Task<IEnumerable<HistoricoProcessoDto>> GetHistoricosByProcessoIdAsync(int processoId);
        Task<HistoricoProcessoDto> GetHistoricoByIdAsync(int processoId, int id);
        Task<HistoricoProcessoDto> CreateHistoricoAsync(int processoId, CreateHistoricoProcessoDto createHistoricoDto);
        Task UpdateHistoricoAsync(int processoId, int id, UpdateHistoricoProcessoDto updateHistoricoDto);
        Task DeleteHistoricoAsync(int processoId, int id);
    }
}
