
using ProcessControl.Application.DTOs;

namespace ProcessControl.Application.Interfaces
{
    public interface IHistoricoProcessoService
    {
        Task<IEnumerable<HistoricoProcessoDto>> GetHistoricosByProcessoIdAsync(int page, int? limit, int processoId);
        Task<HistoricoProcessoDto> CreateHistoricoAsync(int processoId, CreateHistoricoProcessoDto createHistoricoDto);
        Task UpdateHistoricoAsync(int processoId, int id, UpdateHistoricoProcessoDto updateHistoricoDto);
        Task DeleteHistoricoAsync(int processoId, int id);
    }
}
