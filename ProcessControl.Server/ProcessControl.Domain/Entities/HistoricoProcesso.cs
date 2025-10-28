namespace ProcessControl.Domain.Entities
{
    public class HistoricoProcesso
    {
        public int Id { get; set; }

        public int ProcessoId { get; set; }

        public required Processo Processo { get; set; }

        public required string Descricao { get; set; }

        public DateTime DataInclusao { get; set; } = DateTime.UtcNow;

        public DateTime DataAlteracao { get; set; }
    }
}
