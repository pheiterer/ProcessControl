using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.Infrastructure.Repositories
{
    public sealed class ProcessoRepository(ApplicationDbContext context) : IProcessoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Processo>> GetProcessListAsync(int page, int limit, string? searchTerm)
        {
            var query = _context.Processos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.NumeroProcesso.Contains(searchTerm));

            query = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * limit)
                .Take(limit);

            return await query.ToListAsync();
        }

        public async Task<Processo?> GetByIdAsync(int id) =>
            await _context.Processos.FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Processo processo)
        {
            await _context.Processos.AddAsync(processo);
        }

        public async Task DeleteAsync(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo != null)
            {
                _context.Processos.Remove(processo);
            }
        }

        public async Task<Processo?> GetByNumeroProcessoAsync(string numeroProcesso) =>
            await _context.Processos.AsNoTracking().FirstOrDefaultAsync(p => p.NumeroProcesso == numeroProcesso);
    }
}
