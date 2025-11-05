namespace ProcessControl.Domain.Entities
{
    public sealed class HistoricoProcesso
    {
        public int Id { get; private set; }

        public int ProcessoId { get; private set; }

        public Processo Processo { get; private set; } = null!;

        public string Descricao { get; private set; } = null!;

        public DateTime DataInclusao { get; set; }

        public DateTime DataAlteracao { get; set; }

        private HistoricoProcesso() { }

        public HistoricoProcesso(int processoId, string descricao)
        {
            ProcessoId = processoId;
            Descricao = descricao;
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            Descricao = novaDescricao;
        }
    }
}
