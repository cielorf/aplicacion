using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public GeneroController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerGeneros")]
        public IActionResult VerGeneros()
        {
            var generos = _context.Generos.ToList();
            return Ok(generos);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Genero genero)
        {
            _context.Generos.Add(genero);
            _context.SaveChanges();
            return Ok("Genero agregado correctamente");
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Genero datos)
        {
            var genero = _context.Generos.FirstOrDefault(p => p.IdGenero == id);

            if (genero == null)
                return NotFound("Genero no encontrada");

            genero.Nombre = datos.Nombre;
            genero.Estado = datos.Estado;

            _context.SaveChanges();
            return Ok("Genero actualizado correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var genero = _context.Generos.FirstOrDefault(p => p.IdGenero == id);

            if (genero == null)
                return NotFound("Genero no encontrado");

            // Borrado lógico
            genero.Estado = 2;

            _context.SaveChanges();

            return Ok("Genero eliminado correctamente");
        }
    }
}
