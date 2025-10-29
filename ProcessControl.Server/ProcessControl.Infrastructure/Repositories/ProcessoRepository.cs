using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.Infrastructure.Repositories
{
    public sealed class ProcessoRepository(ApplicationDbContext context) : IProcessoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Processo>> GetProcessListAsync(int page, int limit, string? numeroProcesso)
        {
            var query = _context.Processos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(numeroProcesso))
                query = query.Where(p => p.NumeroProcesso.Contains(numeroProcesso));

            query = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * limit)
                .Take(limit);

            return await query.ToListAsync();
        }

        public async Task<Processo?> GetByIdAsync(int id) => await _context.Processos.FindAsync(id);

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
