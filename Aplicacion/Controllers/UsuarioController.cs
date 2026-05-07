using Backend.Data.Models;
using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly BdenviusaContext _context;
        private readonly ILogger<SerieController> _log;

        public UsuarioController(BdenviusaContext context, ILogger<SerieController> log)
        {
            _context = context;
            _log = log;
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
                _log.LogInformation("Agregando un nuevo usuario: {Nombre}", usuario.Nombre);
                _context.Usuarios.Add(usuario);
                _log.LogInformation("Guardando cambios en la base de datos para el usuario: {Nombre}", usuario.Nombre);
                _context.SaveChanges();
                _log.LogInformation("Usuario agregado correctamente:  {Nombre}", usuario.Nombre);
                return Ok("Usuario agregado correctamente");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar el usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Usuario datos)
        {
           try
            {
                _log.LogInformation("Editando el usuario con ID: {Id}", id);
                var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                usuario.Nombre = datos.Nombre;
                usuario.ApellidoPaterno = datos.ApellidoPaterno;
                usuario.ApellidoMaterno = datos.ApellidoMaterno;
                usuario.Telefono = datos.Telefono;
                usuario.Correo = datos.Correo;
                usuario.Estado = datos.Estado;
                usuario.IdRol = datos.IdRol;
                _log.LogInformation("Película editada correctamente: {Titulo}", usuario.Nombre);
                _context.SaveChanges();
                return Ok("Usuario actualizado correctamente");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al editar el usuario");
                return StatusCode(500, "Error interno del servidor");
            }
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