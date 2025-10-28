
using ProcessControl.Application.DTOs;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Services
{
    public class ProcessoService(IProcessoRepository processoRepository) : IProcessoService
    {
        private readonly IProcessoRepository _processoRepository = processoRepository;

        public async Task<IEnumerable<ProcessoDto>> GetAllProcessosAsync(string? numeroProcesso)
        {
            var processos = string.IsNullOrEmpty(numeroProcesso)
                ? await _processoRepository.GetAllAsync()
                : await _processoRepository.GetByNumeroProcessoAsync(numeroProcesso);

            return processos.Select(p => new ProcessoDto
            {
                Id = p.Id,
                NumeroProcesso = p.NumeroProcesso,
                Autor = p.Autor,
                Reu = p.Reu,
                DataAjuizamento = p.DataAjuizamento,
                Status = p.Status,
                Descricao = p.Descricao
            });
        }

        public async Task<ProcessoDto> GetProcessoByIdAsync(int id)
        {
            var processo = await _processoRepository.GetByIdAsync(id);
            if (processo == null) return null;

            return new ProcessoDto
            {
                Id = processo.Id,
                NumeroProcesso = processo.NumeroProcesso,
                Autor = processo.Autor,
                Reu = processo.Reu,
                DataAjuizamento = processo.DataAjuizamento,
                Status = processo.Status,
                Descricao = processo.Descricao
            };
        }

        public async Task<ProcessoDto> CreateProcessoAsync(CreateProcessoDto createProcessoDto)
        {
            var processo = new Processo
            {
                NumeroProcesso = createProcessoDto.NumeroProcesso,
                Autor = createProcessoDto.Autor,
                Reu = createProcessoDto.Reu,
                Descricao = createProcessoDto.Descricao,
                DataAjuizamento = DateTime.UtcNow,
                Status = StatusProcesso.EmAndamento
            };

            await _processoRepository.AddAsync(processo);

            return new ProcessoDto
            {
                Id = processo.Id,
                NumeroProcesso = processo.NumeroProcesso,
                Autor = processo.Autor,
                Reu = processo.Reu,
                DataAjuizamento = processo.DataAjuizamento,
                Status = processo.Status,
                Descricao = processo.Descricao
            };
        }

        public async Task UpdateProcessoAsync(int id, UpdateProcessoDto updateProcessoDto)
        {
            var processo = await _processoRepository.GetByIdAsync(id);
            if (processo == null) return;

            processo.NumeroProcesso = updateProcessoDto.NumeroProcesso;
            processo.Autor = updateProcessoDto.Autor;
            processo.Reu = updateProcessoDto.Reu;
            processo.Status = updateProcessoDto.Status;
            processo.Descricao = updateProcessoDto.Descricao;

            await _processoRepository.UpdateAsync(processo);
        }

        public async Task DeleteProcessoAsync(int id)
        {
            await _processoRepository.DeleteAsync(id);
        }
    }
}
