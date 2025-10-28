

using ProcessControl.Application.DTOs;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Services
{
    public class HistoricoProcessoService(IHistoricoProcessoRepository historicoRepository, IProcessoRepository processoRepository) : IHistoricoProcessoService
    {
        private readonly IHistoricoProcessoRepository _historicoRepository = historicoRepository;
        private readonly IProcessoRepository _processoRepository = processoRepository;

        public async Task<IEnumerable<HistoricoProcessoDto>> GetHistoricosByProcessoIdAsync(int processoId)
        {
            var historicos = await _historicoRepository.GetByProcessoIdAsync(processoId);
            return historicos.Select(h => new HistoricoProcessoDto
            {
                Id = h.Id,
                ProcessoId = h.ProcessoId,
                Descricao = h.Descricao,
                DataInclusao = h.DataInclusao,
                DataAlteracao = h.DataAlteracao
            });
        }

        public async Task<HistoricoProcessoDto> GetHistoricoByIdAsync(int processoId, int id)
        {
            var historico = await _historicoRepository.GetByIdAsync(processoId, id);
            if (historico == null) return null;

            return new HistoricoProcessoDto
            {
                Id = historico.Id,
                ProcessoId = historico.ProcessoId,
                Descricao = historico.Descricao,
                DataInclusao = historico.DataInclusao,
                DataAlteracao = historico.DataAlteracao
            };
        }

        public async Task<HistoricoProcessoDto> CreateHistoricoAsync(int processoId, CreateHistoricoProcessoDto createHistoricoDto)
        {
            var processo = await _processoRepository.GetByIdAsync(processoId);
            if (processo == null) return null; // Ou lançar uma exceção

            var historico = new HistoricoProcesso
            {
                ProcessoId = processoId,
                Processo = processo,
                Descricao = createHistoricoDto.Descricao,
                DataInclusao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

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
            if (historico == null) return;

            historico.Descricao = updateHistoricoDto.Descricao;
            historico.DataAlteracao = DateTime.UtcNow;

            await _historicoRepository.UpdateAsync(historico);
        }

        public async Task DeleteHistoricoAsync(int processoId, int id)
        {
            var historico = await _historicoRepository.GetByIdAsync(processoId, id);
            if (historico != null)
            {
                await _historicoRepository.DeleteAsync(historico.Id);
            }
        }
    }
}
