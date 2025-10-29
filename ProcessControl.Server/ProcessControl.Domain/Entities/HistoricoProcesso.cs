namespace ProcessControl.Domain.Entities
{
    public sealed class HistoricoProcesso
    {
        public int Id { get; set; }

        public required int ProcessoId { get; set; }

        public required Processo Processo { get; set; }

        public required string Descricao { get; set; }

        public DateTime DataInclusao { get; set; } = DateTime.UtcNow;

        public DateTime DataAlteracao { get; set; }
    }
}
