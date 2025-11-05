using ProcessControl.Application.Interfaces;
using ProcessControl.Infrastructure.Persistence;
using ProcessControl.Infrastructure.Repositories;

namespace ProcessControl.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProcessoRepository ProcessoRepository { get; }
        public IHistoricoProcessoRepository HistoricoProcessoRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProcessoRepository = new ProcessoRepository(_context);
            HistoricoProcessoRepository = new HistoricoProcessoRepository(_context);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
