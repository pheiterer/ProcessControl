
using System.ComponentModel.DataAnnotations;
using ProcessControl.Domain.Entities;

namespace ProcessControl.Application.DTOs
{
    public class UpdateProcessoDto
    {
        [Required]
        public required string NumeroProcesso { get; set; }

        [Required]
        public required string Autor { get; set; }

        [Required]
        public required string Reu { get; set; }

        public StatusProcesso Status { get; set; }

        public string? Descricao { get; set; }
    }
}
