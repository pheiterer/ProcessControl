
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
        public async Task<ActionResult<IEnumerable<HistoricoProcessoDto>>> GetHistoricos(int processoId)
        {
            var historicos = await _historicoService.GetHistoricosByProcessoIdAsync(processoId);
            return Ok(historicos);
        }

        // GET: api/processos/5/historicos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricoProcessoDto>> GetHistorico(int processoId, int id)
        {
            var historico = await _historicoService.GetHistoricoByIdAsync(processoId, id);
            if (historico == null)
            {
                return NotFound();
            }
            return Ok(historico);
        }

        // PUT: api/processos/5/historicos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorico(int processoId, int id, UpdateHistoricoProcessoDto updateHistoricoDto)
        {
            await _historicoService.UpdateHistoricoAsync(processoId, id, updateHistoricoDto);
            return NoContent();
        }

        // POST: api/processos/5/historicos
        [HttpPost]
        public async Task<ActionResult<HistoricoProcessoDto>> PostHistorico(int processoId, CreateHistoricoProcessoDto createHistoricoDto)
        {
            var novoHistorico = await _historicoService.CreateHistoricoAsync(processoId, createHistoricoDto);
            return CreatedAtAction(nameof(GetHistorico), new { processoId, id = novoHistorico.Id }, novoHistorico);
        }

        // DELETE: api/processos/5/historicos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorico(int processoId, int id)
        {
            await _historicoService.DeleteHistoricoAsync(processoId, id);
            return NoContent();
        }
    }
}
