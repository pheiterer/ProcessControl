
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcessControl.Domain.Entities;
using ProcessControl.Infrastructure.Persistence;

namespace ProcessControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessosController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/Processos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Processo>>> GetProcessos([FromQuery] string numeroProcesso)
        {
            if (!string.IsNullOrEmpty(numeroProcesso))
            {
                return await _context.Processos.Where(p => p.NumeroProcesso.Contains(numeroProcesso)).ToListAsync();
            }
            return await _context.Processos.ToListAsync();
        }

        // GET: api/Processos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Processo>> GetProcesso(int id)
        {
            var processo = await _context.Processos.FindAsync(id);

            if (processo == null)
            {
                return NotFound();
            }

            return processo;
        }

        // PUT: api/Processos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcesso(int id, Processo processo)
        {
            if (id != processo.Id)
            {
                return BadRequest();
            }

            _context.Entry(processo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessoExists(id))
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

        // POST: api/Processos
        [HttpPost]
        public async Task<ActionResult<Processo>> PostProcesso(Processo processo)
        {
            _context.Processos.Add(processo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProcesso", new { id = processo.Id }, processo);
        }

        // DELETE: api/Processos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcesso(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            _context.Processos.Remove(processo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcessoExists(int id)
        {
            return _context.Processos.Any(e => e.Id == id);
        }
    }
}
