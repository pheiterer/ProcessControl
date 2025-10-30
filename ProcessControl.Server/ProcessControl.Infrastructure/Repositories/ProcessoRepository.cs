using Microsoft.EntityFrameworkCore;
using ProcessControl.Application.Interfaces;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;
using ProcessControl.Application.Exceptions;

namespace ProcessControl.Infrastructure.Repositories
{
    public sealed class ProcessoRepository(ApplicationDbContext context) : IProcessoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Processo>> GetProcessListAsync(int page, int limit, string? searchTerm)
        {
            var query = _context.Processos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.NumeroProcesso.Contains(searchTerm));

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
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    throw new DuplicateEntryException($"Processo with NumeroProcesso {processo.NumeroProcesso} already exists.", pgEx);
                }
                throw; // Re-throw other DbUpdateExceptions
            }
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

        public async Task<Processo?> GetByNumeroProcessoAsync(string numeroProcesso) =>
            await _context.Processos.FirstOrDefaultAsync(p => p.NumeroProcesso == numeroProcesso);
    }
}
