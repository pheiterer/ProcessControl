
using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.Infrastructure.Repositories
{
    public class HistoricoProcessoRepository(ApplicationDbContext context) : IHistoricoProcessoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<HistoricoProcesso>> GetByProcessoIdAsync(int processoId)
        {
            return await _context.HistoricosProcesso.Where(h => h.ProcessoId == processoId).ToListAsync();
        }

        public async Task<HistoricoProcesso> GetByIdAsync(int processoId, int id)
        {
            return await _context.HistoricosProcesso.FirstOrDefaultAsync(h => h.ProcessoId == processoId && h.Id == id);
        }

        public async Task AddAsync(HistoricoProcesso historico)
        {
            _context.HistoricosProcesso.Add(historico);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HistoricoProcesso historico)
        {
            _context.Entry(historico).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var historico = await _context.HistoricosProcesso.FindAsync(id);
            if (historico != null)
            {
                _context.HistoricosProcesso.Remove(historico);
                await _context.SaveChangesAsync();
            }
        }
    }
}
