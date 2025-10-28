
using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models
{
    public class HistoricoProcesso
    {
        public int Id { get; set; }

        public int ProcessoId { get; set; }

        public Processo Processo { get; set; }

        [Required]
        public string Descricao { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
