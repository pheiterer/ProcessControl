
using ProcessControl.Application.DTOs;

namespace ProcessControl.Application.Interfaces
{
    public interface IProcessoService
    {
        Task<IEnumerable<ProcessoDto>> GetProcessListAsync(int page, int? limit, string? searchTerm);
        Task<ProcessoDto?> GetProcessoByIdAsync(int id);
        Task<ProcessoDto> CreateProcessoAsync(CreateProcessoDto createProcessoDto);
        Task UpdateProcessoAsync(int id, UpdateProcessoDto updateProcessoDto);
        Task DeleteProcessoAsync(int id);
    }
}
