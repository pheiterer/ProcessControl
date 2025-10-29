namespace ProcessControl.Domain.Entities
{
    public enum StatusProcesso
    {
        EmAndamento,
        Suspenso,
        Encerrado
    }

    public sealed class Processo
    {
        public int Id { get; set; }

        public required string NumeroProcesso { get; set; }

        public required string Autor { get; set; }

        public required string Reu { get; set; }

        public DateTime DataAjuizamento { get; set; }

        public StatusProcesso Status { get; set; }

        public string? Descricao { get; set; }

        public ICollection<HistoricoProcesso>? Historico { get; set; }
    }
}
