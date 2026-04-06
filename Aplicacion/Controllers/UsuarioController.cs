using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public UsuarioController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerUsuarios")]
        public IActionResult VerUsuarios()
        {
          
            var usuarios = _context.Usuarios.Where(u => u.Estado == 1).ToList();
            return Ok(usuarios);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Usuario usuario)
        {
            try
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return Ok("Usuario agregado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar usuario: {ex.Message}");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Usuario datos)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            usuario.Nombre = datos.Nombre;
            usuario.ApellidoPaterno = datos.ApellidoPaterno;
            usuario.ApellidoMaterno = datos.ApellidoMaterno;
            usuario.Telefono = datos.Telefono;
            usuario.Correo = datos.Correo;
            usuario.Estado = datos.Estado;

            _context.SaveChanges();
            return Ok("Usuario actualizado correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Borrado lógico (Estado = 2)
            usuario.Estado = 2;

            _context.SaveChanges();
            return Ok("Usuario eliminado correctamente");
        }
    }
}