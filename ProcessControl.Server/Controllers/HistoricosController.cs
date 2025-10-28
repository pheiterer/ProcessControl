
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.API.Controllers
{
    [Route("api/processos/{processoId}/historicos")]
    [ApiController]
    public class HistoricosController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/processos/5/historicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricoProcesso>>> GetHistoricos(int processoId)
        {
            return await _context.HistoricosProcesso.Where(h => h.ProcessoId == processoId).ToListAsync();
        }

        // GET: api/processos/5/historicos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricoProcesso>> GetHistorico(int processoId, int id)
        {
            var historico = await _context.HistoricosProcesso.FirstOrDefaultAsync(h => h.ProcessoId == processoId && h.Id == id);

            if (historico == null)
            {
                return NotFound();
            }

            return historico;
        }

        // PUT: api/processos/5/historicos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorico(int processoId, int id, HistoricoProcesso historico)
        {
            if (id != historico.Id || processoId != historico.ProcessoId)
            {
                return BadRequest();
            }

            historico.DataAlteracao = DateTime.UtcNow;
            _context.Entry(historico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/processos/5/historicos
        [HttpPost]
        public async Task<ActionResult<HistoricoProcesso>> PostHistorico(int processoId, HistoricoProcesso historico)
        {
            historico.ProcessoId = processoId;
            historico.DataInclusao = DateTime.UtcNow;
            historico.DataAlteracao = DateTime.UtcNow;
            _context.HistoricosProcesso.Add(historico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistorico", new { processoId, id = historico.Id }, historico);
        }

        // DELETE: api/processos/5/historicos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorico(int processoId, int id)
        {
            var historico = await _context.HistoricosProcesso.FirstOrDefaultAsync(h => h.ProcessoId == processoId && h.Id == id);
            if (historico == null)
            {
                return NotFound();
            }

            _context.HistoricosProcesso.Remove(historico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistoricoExists(int id)
        {
            return _context.HistoricosProcesso.Any(e => e.Id == id);
        }
    }
}
