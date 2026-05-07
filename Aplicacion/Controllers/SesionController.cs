using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public SesionController(BdenviusaContext context)
        {
            _context = context;
        }

       
        [HttpGet("VerSesiones")]
        public IActionResult VerSesiones()
        {
            var lista = _context.Sesions.ToList();
            return Ok(lista);
        }

        // 2. Ver sesiones de un usuario específico
        [HttpGet("HistorialUsuario/{idUsuario}")]
        public IActionResult VerPorUsuario(short idUsuario)
        {
            var sesiones = _context.Sesions
                .Where(s => s.IdUsuario == idUsuario)
                .ToList();

            if (!sesiones.Any())
                return NotFound("No hay historial para este usuario");

            return Ok(sesiones);
        }

  
        [HttpPost("Registrar")]
        public IActionResult Registrar([FromBody] Sesion sesion)
        {
            try
            {

                _context.Sesions.Add(sesion);
                _context.SaveChanges();

                return Ok("Sesión registrada exitosamente");
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Error de SQL: {errorReal}");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Sesion sesionActualizada)
        {
            
            if (id != sesionActualizada.IdSesion) return BadRequest("Los IDs no coinciden");

            // 2. Buscar el registro original
            var sesion = _context.Sesions.Find(id);
            if (sesion == null) return NotFound();

            // 3. Actualizar campos
            sesion.Nickname = sesionActualizada.Nickname;
            sesion.Contraseña = sesionActualizada.Contraseña;
            sesion.IdUsuario = sesionActualizada.IdUsuario;

            _context.SaveChanges();
            return Ok("Registro actualizado");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var sesion = _context.Sesions.FirstOrDefault(s => s.IdSesion == id);

            if (sesion == null)
                return NotFound("No se encontró el registro de sesión");

            _context.Sesions.Remove(sesion);

            _context.SaveChanges();
            return Ok("La sesión ha sido eliminada exitosamente");
        }

        
    }
}