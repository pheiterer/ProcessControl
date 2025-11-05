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
        public int Id { get; private set; }

        public string NumeroProcesso { get; private set; }

        public string Autor { get; private set; }

        public string Reu { get; private set; }

        public DateTime DataAjuizamento { get; private set; }

        public StatusProcesso Status { get; private set; }

        public string? Descricao { get; private set; }

        public ICollection<HistoricoProcesso> Historico { get; private set; } = new List<HistoricoProcesso>();

        private Processo() { }

        public Processo(string numeroProcesso, string autor, string reu, DateTime dataAjuizamento, string? descricao)
        {
            NumeroProcesso = numeroProcesso;
            Autor = autor;
            Reu = reu;
            DataAjuizamento = dataAjuizamento;
            Descricao = descricao;
            Status = StatusProcesso.EmAndamento;

            AdicionarHistorico("Processo criado e iniciado.");
        }

        public void AtualizarDadosBasicos(string numeroProcesso, string autor, string reu, string? descricao)
        {
            NumeroProcesso = numeroProcesso;
            Autor = autor;
            Reu = reu;
            Descricao = descricao;

            AdicionarHistorico("Dados básicos do processo foram atualizados.");
        }

        public void MudarStatus(StatusProcesso novoStatus)
        {
            if (Status == novoStatus) return;

            if (Status == StatusProcesso.Encerrado)
            {
                throw new InvalidOperationException("Não é possível alterar o status de um processo já encerrado.");
            }

            Status = novoStatus;
            AdicionarHistorico($"Status do processo alterado para: {novoStatus}");
        }

        private void AdicionarHistorico(string descricao)
        {
            var novoHistorico = new HistoricoProcesso(this.Id, descricao);
            Historico.Add(novoHistorico);
        }
    }
}
