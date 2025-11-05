using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.DTOs;
using ProcessControl.Application.Exceptions;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.Services
{
    public sealed class ProcessoService(IUnitOfWork unitOfWork, IMapper mapper) : IProcessoService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ProcessoDto>> GetProcessListAsync(int page, int? limit, string? searchTerm)
        {
            const int defaultLimit = 10;
            const int maxLimit = 50;

            if (page < 1)
                page = 1;

            int pageSize = limit.HasValue && limit.Value > 0
                ? Math.Min(limit.Value, maxLimit)
                : defaultLimit;

            var processos = await _unitOfWork.ProcessoRepository.GetProcessListAsync(page, pageSize, searchTerm);

            return _mapper.Map<IEnumerable<ProcessoDto>>(processos);
        }

        public async Task<ProcessoDto> GetProcessoByIdAsync(int id)
        {
            var processo = await _unitOfWork.ProcessoRepository.GetByIdAsync(id);
            if (processo == null) throw new NotFoundException($"Processo with ID {id} not found.");

            return _mapper.Map<ProcessoDto>(processo);
        }

        public async Task<ProcessoDto> CreateProcessoAsync(CreateProcessoDto createProcessoDto)
        {
            var processo = _mapper.Map<Processo>(createProcessoDto);

            await _unitOfWork.ProcessoRepository.AddAsync(processo);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is Npgsql.PostgresException pgEx)
                {
                    switch (pgEx.SqlState)
                    {
                        case "23505": // unique_violation
                            throw new DuplicateEntryException($"Já existe um processo com o número '{processo.NumeroProcesso}'.", pgEx);

                        case "23503": // foreign_key_violation
                            throw new ForeignKeyViolationException("Uma das referências de dados fornecidas é inválida.", pgEx);
                    }
                }
                throw; // Re-throw other DbUpdateExceptions
            }

            return _mapper.Map<ProcessoDto>(processo);
        }

        public async Task UpdateProcessoAsync(int id, UpdateProcessoDto updateProcessoDto)
        {
            var processo = await _unitOfWork.ProcessoRepository.GetByIdAsync(id);
            if (processo == null) throw new NotFoundException($"Processo with ID {id} not found.");

            // Mapeia as propriedades do DTO para a entidade que já está sendo rastreada pelo EF Core
            _mapper.Map(updateProcessoDto, processo);

            await _unitOfWork.ProcessoRepository.UpdateAsync(processo);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProcessoAsync(int id)
        {
            var processo = await _unitOfWork.ProcessoRepository.GetByIdAsync(id);
            if (processo == null) throw new NotFoundException($"Processo with ID {id} not found.");
            
            await _unitOfWork.ProcessoRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
