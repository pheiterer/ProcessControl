using ProcessControl.Application.DTOs;
using ProcessControl.Application.Interfaces;
using ProcessControl.Application.Exceptions;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Services
{
    public sealed class ProcessoService(IProcessoRepository processRepository) : IProcessoService
    {
        private readonly IProcessoRepository _processRepository = processRepository;

        public async Task<IEnumerable<ProcessoDto>> GetProcessListAsync(int page, int? limit, string? searchTerm)
        {
            const int defaultLimit = 10;
            const int maxLimit = 50;

            if (page < 1)
                page = 1;

            int pageSize = limit.HasValue && limit.Value > 0
                ? Math.Min(limit.Value, maxLimit)
                : defaultLimit;

            var processos = await _processRepository.GetProcessListAsync(page, pageSize, searchTerm);

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
            var processo = await _processRepository.GetByIdAsync(id);
            if (processo == null) throw new NotFoundException($"Processo with ID {id} not found.");

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
                DataAjuizamento = createProcessoDto.DataAjuizamento.ToUniversalTime(),
                Status = createProcessoDto.Status
            };

            await _processRepository.AddAsync(processo);

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
            var processo = await _processRepository.GetByIdAsync(id);
            if (processo == null) throw new NotFoundException($"Processo with ID {id} not found.");

            processo.NumeroProcesso = updateProcessoDto.NumeroProcesso;
            processo.Autor = updateProcessoDto.Autor;
            processo.Reu = updateProcessoDto.Reu;
            processo.Status = updateProcessoDto.Status;
            processo.Descricao = updateProcessoDto.Descricao;

            await _processRepository.UpdateAsync(processo);
        }

        public async Task DeleteProcessoAsync(int id)
        {
            var processo = await _processRepository.GetByIdAsync(id);
            if (processo == null) throw new NotFoundException($"Processo with ID {id} not found.");
            await _processRepository.DeleteAsync(id);
        }
    }
}
