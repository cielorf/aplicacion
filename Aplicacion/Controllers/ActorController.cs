using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public ActorController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerActores")]
        public IActionResult VerActores()
        {
          
            var actores = _context.Actors.Where(a => a.Estado == 1).ToList();
            return Ok(actores);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Actor actor)
        {
            try
            {
                _context.Actors.Add(actor);
                _context.SaveChanges();
                return Ok("Actor agregado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar actor: {ex.Message}");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Actor datos)
        {
            // Buscamos por id_actor 
            var actor = _context.Actors.FirstOrDefault(a => a.IdActor == id);

            if (actor == null)
                return NotFound("Actor no encontrado");

            // Mapeo de campos 
            actor.Nombre = datos.Nombre;
            actor.ApellidoPaterno = datos.ApellidoPaterno;
            actor.ApellidoMaterno = datos.ApellidoMaterno;
            actor.FechaNac = datos.FechaNac;
            actor.Poster = datos.Poster;
            actor.Estado = datos.Estado;

            _context.SaveChanges();
            return Ok("Actor actualizado correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var actor = _context.Actors.FirstOrDefault(a => a.IdActor == id);

            if (actor == null)
                return NotFound("Actor no encontrado");

            // Borrado lógico (Estado = 2)
            actor.Estado = 2;

            _context.SaveChanges();
            return Ok("Actor eliminado correctamente");
        }
    }
}
