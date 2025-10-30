
using Microsoft.AspNetCore.Mvc;
using ProcessControl.Application.DTOs;
using ProcessControl.Application.Interfaces;

namespace ProcessControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessosController(IProcessoService processService) : ControllerBase
    {
        private readonly IProcessoService _processService = processService;

        // GET: api/Processos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessoDto>>> GetProcessos(
            [FromQuery] int page = 1,
            [FromQuery] int? limit = null,
            [FromQuery] string? searchTerm = null)
        {
            var processos = await _processService.GetProcessListAsync(page, limit, searchTerm);
            return Ok(processos);
        }

        // GET: api/Processos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessoDto>> GetProcesso([FromRoute] int id)
        {
            var processo = await _processService.GetProcessoByIdAsync(id);
            if (processo == null)
            {
                return NotFound();
            }
            return Ok(processo);
        }

        // PUT: api/Processos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcesso([FromRoute] int id, UpdateProcessoDto updateProcessoDto)
        {
            await _processService.UpdateProcessoAsync(id, updateProcessoDto);
            return NoContent();
        }

        // POST: api/Processos
        [HttpPost]
        public async Task<ActionResult<ProcessoDto>> PostProcesso(CreateProcessoDto createProcessoDto)
        {
            var novoProcesso = await _processService.CreateProcessoAsync(createProcessoDto);
            return CreatedAtAction(nameof(GetProcesso), new { id = novoProcesso.Id }, novoProcesso);
        }

        // DELETE: api/Processos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcesso([FromRoute] int id)
        {
            await _processService.DeleteProcessoAsync(id);
            return NoContent();
        }
    }
}
