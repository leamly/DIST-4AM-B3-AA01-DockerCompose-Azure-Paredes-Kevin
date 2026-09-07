using InventarioA.Api.Data;
using InventarioA.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioA.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 

    public class InventarioController: ControllerBase
    {
       private readonly InventarioDBContext _context;
        public InventarioController(InventarioDBContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Administrador, Usuario")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventario>>> GetInventarios()
        {
            var inventarios = await _context.Inventarios.AsNoTracking().ToListAsync();
            return Ok(inventarios);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventario>> GetInventario(int id)
        {
            var inventario = await _context.Inventarios.AsNoTracking().FirstOrDefaultAsync(i => i.IdInventario == id);
            if (inventario == null)
            {
                return NotFound();
            }
            return Ok(inventario);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Inventario>> CreateInventario(Inventario inventario)
        {
            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventario), new { id = inventario.IdInventario }, inventario);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Inventario>> UpdateInventario(int id, Inventario inventario)
        {
            if (id != inventario.IdInventario)
            {
                return BadRequest();
            }

            _context.Entry(inventario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventario(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            _context.Inventarios.Remove(inventario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
