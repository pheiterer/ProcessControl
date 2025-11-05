
using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.Infrastructure.Repositories
{
    public sealed class HistoricoProcessoRepository(ApplicationDbContext context) : IHistoricoProcessoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<HistoricoProcesso>> GetByProcessoIdAsync(int page, int limit, int processoId)
        {
            var query = _context.HistoricosProcesso
                .AsNoTracking()
                .Where(h => h.ProcessoId == processoId)
                .OrderByDescending(h => h.DataInclusao)
                .Skip((page - 1) * limit)
                .Take(limit);

            return await query.ToListAsync();
        }

        public async Task<HistoricoProcesso?> GetByIdAsync(int processoId, int id)
        {
            return await _context.HistoricosProcesso
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.ProcessoId == processoId && h.Id == id);
        }

        public async Task AddAsync(HistoricoProcesso historico)
        {
            await _context.HistoricosProcesso.AddAsync(historico);
        }

        public Task UpdateAsync(HistoricoProcesso historico)
        {
            _context.Entry(historico).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var historico = await _context.HistoricosProcesso.FindAsync(id);
            if (historico != null)
            {
                _context.HistoricosProcesso.Remove(historico);
            }
        }
    }
}
