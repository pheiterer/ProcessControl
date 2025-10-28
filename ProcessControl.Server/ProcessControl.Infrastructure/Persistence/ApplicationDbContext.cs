
using Microsoft.EntityFrameworkCore;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Processo> Processos { get; set; }
        public DbSet<HistoricoProcesso> HistoricosProcesso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Processo>()
                .HasIndex(p => p.NumeroProcesso)
                .IsUnique();

            modelBuilder.Entity<HistoricoProcesso>()
                .HasOne(h => h.Processo)
                .WithMany(p => p.Historico)
                .HasForeignKey(h => h.ProcessoId);
        }
    }
}
