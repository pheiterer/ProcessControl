
using Microsoft.EntityFrameworkCore;
using ProcessControl.Server.Models;

namespace ProcessControl.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        
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
