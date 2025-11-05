using ProcessControl.Application.DTOs;
using ProcessControl.Application.Exceptions;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Services
{
    public sealed class HistoricoProcessoService(IUnitOfWork unitOfWork) : IHistoricoProcessoService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<HistoricoProcessoDto>> GetHistoricosByProcessoIdAsync(int page, int? limit, int processoId)
        {
            const int defaultLimit = 10;
            const int maxLimit = 50;

            if (page < 1)
                page = 1;

            int pageSize = limit.HasValue && limit.Value > 0
                ? Math.Min(limit.Value, maxLimit)
                : defaultLimit;

            var historicos = await _unitOfWork.HistoricoProcessoRepository.GetByProcessoIdAsync(page, pageSize, processoId);
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
            var processo = await _unitOfWork.ProcessoRepository.GetByIdAsync(processoId);
            if (processo == null) throw new NotFoundException($"Processo with ID {processoId} not found.");

            var historico = new HistoricoProcesso(processoId, createHistoricoDto.Descricao);

            await _unitOfWork.HistoricoProcessoRepository.AddAsync(historico);
            await _unitOfWork.SaveChangesAsync();

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
            var historico = await _unitOfWork.HistoricoProcessoRepository.GetByIdAsync(processoId, id);
            if (historico == null) throw new NotFoundException($"Historico with ID {id} for Processo {processoId} not found.");

            historico.AtualizarDescricao(updateHistoricoDto.Descricao);

            await _unitOfWork.HistoricoProcessoRepository.UpdateAsync(historico);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteHistoricoAsync(int processoId, int id)
        {
            var historico = await _unitOfWork.HistoricoProcessoRepository.GetByIdAsync(processoId, id);
            if (historico == null) throw new NotFoundException($"Historico with ID {id} for Processo {processoId} not found.");
            
            await _unitOfWork.HistoricoProcessoRepository.DeleteAsync(historico.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
