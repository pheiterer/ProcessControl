
namespace ProcessControl.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProcessoRepository ProcessoRepository { get; }
        IHistoricoProcessoRepository HistoricoProcessoRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
