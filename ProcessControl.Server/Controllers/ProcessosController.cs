
using Microsoft.AspNetCore.Mvc;
using ProcessControl.Application.DTOs;
using ProcessControl.Application.Interfaces;

namespace ProcessControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessosController : ControllerBase
    {
        private readonly IProcessoService _processoService;

        public ProcessosController(IProcessoService processoService)
        {
            _processoService = processoService;
        }

        // GET: api/Processos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessoDto>>> GetProcessos([FromQuery] string? numeroProcesso)
        {
            var processos = await _processoService.GetAllProcessosAsync(numeroProcesso);
            return Ok(processos);
        }

        // GET: api/Processos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessoDto>> GetProcesso(int id)
        {
            var processo = await _processoService.GetProcessoByIdAsync(id);
            if (processo == null)
            {
                return NotFound();
            }
            return Ok(processo);
        }

        // PUT: api/Processos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcesso(int id, UpdateProcessoDto updateProcessoDto)
        {
            await _processoService.UpdateProcessoAsync(id, updateProcessoDto);
            return NoContent();
        }

        // POST: api/Processos
        [HttpPost]
        public async Task<ActionResult<ProcessoDto>> PostProcesso(CreateProcessoDto createProcessoDto)
        {
            var novoProcesso = await _processoService.CreateProcessoAsync(createProcessoDto);
            return CreatedAtAction(nameof(GetProcesso), new { id = novoProcesso.Id }, novoProcesso);
        }

        // DELETE: api/Processos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcesso(int id)
        {
            await _processoService.DeleteProcessoAsync(id);
            return NoContent();
        }
    }
}
