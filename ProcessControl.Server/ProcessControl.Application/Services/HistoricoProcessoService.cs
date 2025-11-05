using ProcessControl.Application.DTOs;
using ProcessControl.Application.Exceptions;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Services
{
    public sealed class HistoricoProcessoService(IHistoricoProcessoRepository historicoRepository, IProcessoRepository processoRepository) : IHistoricoProcessoService
    {
        private readonly IHistoricoProcessoRepository _historicoRepository = historicoRepository;
        private readonly IProcessoRepository _processoRepository = processoRepository;

        public async Task<IEnumerable<HistoricoProcessoDto>> GetHistoricosByProcessoIdAsync(int page, int? limit, int processoId)
        {
            const int defaultLimit = 10;
            const int maxLimit = 50;

            if (page < 1)
                page = 1;

            int pageSize = limit.HasValue && limit.Value > 0
                ? Math.Min(limit.Value, maxLimit)
                : defaultLimit;

            var historicos = await _historicoRepository.GetByProcessoIdAsync(page, pageSize, processoId);
            return historicos.Select(h => new HistoricoProcessoDto
            {
                Id = h.Id,
                ProcessoId = h.ProcessoId,
                Descricao = h.Descricao,
                DataInclusao = h.DataInclusao,
                DataAlteracao = h.DataAlteracao
            });
        }

        public async Task<HistoricoProcessoDto> CreateHistoricoAsync(int processoId, CreateHistoricoProcessoDto createHistoricoDto)
        {
            var processo = await _processoRepository.GetByIdAsync(processoId);
            if (processo == null) throw new NotFoundException($"Processo with ID {processoId} not found.");

            var historico = new HistoricoProcesso(processoId, createHistoricoDto.Descricao);

            await _historicoRepository.AddAsync(historico);

            return new HistoricoProcessoDto
            {
                Id = historico.Id,
                ProcessoId = historico.ProcessoId,
                Descricao = historico.Descricao,
                DataInclusao = historico.DataInclusao,
                DataAlteracao = historico.DataAlteracao
            };
        }

        public async Task UpdateHistoricoAsync(int processoId, int id, UpdateHistoricoProcessoDto updateHistoricoDto)
        {
            var historico = await _historicoRepository.GetByIdAsync(processoId, id);
            if (historico == null) throw new NotFoundException($"Historico with ID {id} for Processo {processoId} not found.");

            historico.AtualizarDescricao(updateHistoricoDto.Descricao);

            await _historicoRepository.UpdateAsync(historico);
        }

        public async Task DeleteHistoricoAsync(int processoId, int id)
        {
            var historico = await _historicoRepository.GetByIdAsync(processoId, id);
            if (historico == null) throw new NotFoundException($"Historico with ID {id} for Processo {processoId} not found.");
            await _historicoRepository.DeleteAsync(historico.Id);
        }
    }
}
