using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public RolController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerRoles")]
        public IActionResult VerRoles()
        {
           
            var roles = _context.Rols.ToList();
            return Ok(roles);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Rol rol)
        {
            _context.Rols.Add(rol);
            _context.SaveChanges();
            return Ok("Rol agregado correctamente");
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Rol datos)
        {
            var rol = _context.Rols.FirstOrDefault(r => r.IdRol == id);

            if (rol == null)
                return NotFound("Rol no encontrado");

            // Según tu diagrama, el rol solo tiene 'Nombre'
            rol.Nombre = datos.Nombre;

            _context.SaveChanges();
            return Ok("Rol actualizado correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var rol = _context.Rols.FirstOrDefault(r => r.IdRol == id);

            if (rol == null)
                return NotFound("Rol no encontrado");

            _context.Rols.Remove(rol);

            _context.SaveChanges();

            return Ok("Rol eliminado correctamente");
        }
    }
}