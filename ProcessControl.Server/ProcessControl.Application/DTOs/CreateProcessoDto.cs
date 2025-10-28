
using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Application.DTOs
{
    public class CreateProcessoDto
    {
        [Required]
        public required string NumeroProcesso { get; set; }

        [Required]
        public required string Autor { get; set; }

        [Required]
        public required string Reu { get; set; }

        public string? Descricao { get; set; }
    }
}
