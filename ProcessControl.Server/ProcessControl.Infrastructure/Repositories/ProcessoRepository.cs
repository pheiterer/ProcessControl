
using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.Infrastructure.Repositories
{
    public class ProcessoRepository(ApplicationDbContext context) : IProcessoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Processo>> GetAllAsync()
        {
            return await _context.Processos.ToListAsync();
        }

        public async Task<IEnumerable<Processo>> GetByNumeroProcessoAsync(string numeroProcesso)
        {
            return await _context.Processos.Where(p => p.NumeroProcesso.Contains(numeroProcesso)).ToListAsync();
        }

        public async Task<Processo> GetByIdAsync(int id)
        {
            return await _context.Processos.FindAsync(id);
        }

        public async Task AddAsync(Processo processo)
        {
            _context.Processos.Add(processo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Processo processo)
        {
            _context.Entry(processo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo != null)
            {
                _context.Processos.Remove(processo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
