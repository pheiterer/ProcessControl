namespace ProcessControl.Application.DTOs
{
    public class CreateProcessoDto
    {
        public required string NumeroProcesso { get; set; }

        public required string Autor { get; set; }

        public required string Reu { get; set; }

        public required DateTime DataAjuizamento { get; set; }

        public string? Descricao { get; set; }
    }
}
