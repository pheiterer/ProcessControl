
using Microsoft.AspNetCore.Mvc;
using ProcessControl.Application.DTOs;
using ProcessControl.Application.Interfaces;

namespace ProcessControl.API.Controllers
{
    [Route("api/processos/{processoId}/historicos")]
    [ApiController]
    public class HistoricosController(IHistoricoProcessoService historicoService) : ControllerBase
    {
        private readonly IHistoricoProcessoService _historicoService = historicoService;

        // GET: api/processos/5/historicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricoProcessoDto>>> GetHistoricos([FromRoute] int processoId, [FromQuery] int page, [FromQuery] int? limit)
        {
            var historicos = await _historicoService.GetHistoricosByProcessoIdAsync(page, limit, processoId);
            return Ok(historicos);
        }

        // PUT: api/processos/5/historicos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorico([FromRoute] int processoId, [FromRoute] int id, UpdateHistoricoProcessoDto updateHistoricoDto)
        {
            await _historicoService.UpdateHistoricoAsync(processoId, id, updateHistoricoDto);
            return NoContent();
        }

        // POST: api/processos/5/historicos
        [HttpPost]
        public async Task<ActionResult<HistoricoProcessoDto>> PostHistorico([FromRoute] int processoId, CreateHistoricoProcessoDto createHistoricoDto)
        {
            var novoHistorico = await _historicoService.CreateHistoricoAsync(processoId, createHistoricoDto);
            return CreatedAtAction(nameof(GetHistoricos), new { processoId, id = novoHistorico.Id }, novoHistorico);
        }

        // DELETE: api/processos/5/historicos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorico([FromRoute] int processoId, [FromRoute] int id)
        {
            await _historicoService.DeleteHistoricoAsync(processoId, id);
            return NoContent();
        }
    }
}
