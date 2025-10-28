
using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models
{
    public enum StatusProcesso
    {
        EmAndamento,
        Suspenso,
        Encerrado
    }

    public class Processo
    {
        public int Id { get; set; }

        [Required]
        public string NumeroProcesso { get; set; }

        [Required]
        public string Autor { get; set; }

        [Required]
        public string Reu { get; set; }

        public DateTime DataAjuizamento { get; set; }

        public StatusProcesso Status { get; set; }

        public string Descricao { get; set; }

        public ICollection<HistoricoProcesso> Historico { get; set; }
    }
}
