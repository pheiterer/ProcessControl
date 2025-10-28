
namespace ProcessControl.Application.DTOs
{
    public class HistoricoProcessoDto
    {
        public int Id { get; set; }
        public int ProcessoId { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
