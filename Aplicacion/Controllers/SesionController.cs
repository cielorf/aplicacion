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
                .OrderByDescending(s => s.FechaHora) // Las más recientes primero
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
                // Si no mandan fecha, el sistema asigna la actual automáticamente
                if (sesion.FechaHora == default)
                    sesion.FechaHora = DateTime.Now;

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


        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var sesion = _context.Sesions.FirstOrDefault(s => s.IdSesion == id);

            if (sesion == null)
                return NotFound("No se encontró el registro de sesión");

            // Aplicamos borrado lógico: 1 = Activo, 2 = Eliminado/Inactivo
            sesion.Estado = 2;

            _context.SaveChanges();
            return Ok("La sesión ha sido marcada como eliminada");
        }
    }
}