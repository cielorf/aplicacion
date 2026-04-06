using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class UsuarioDependencies : IUsuarioDependencies
    {
        private readonly ILogger<UsuarioDependencies> _log;
        private readonly BdenviusaContext _context;

        public UsuarioDependencies(ILogger<UsuarioDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Usuario>> GetUsuarios()
        {
            try
            {
                var usuarios = _context.Usuarios
                    .Where(u => u.Estado == 1)
                    .ToList();

                return Result.Success(usuarios);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener la lista de usuarios");
                return Result.Failure<List<Usuario>>(Error.Create("Error al obtener usuarios de la base de datos"));
            }
        }

        public Result<Usuario> GetUsuarioById(short id)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);

                if (usuario == null)
                    return Result.Failure<Usuario>(Error.Create("Usuario no encontrado"));

                return Result.Success(usuario);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al buscar usuario con ID {id}", id);
                return Result.Failure<Usuario>(Error.Create("Error al buscar el usuario"));
            }
        }

        public Result<bool> AddUsuario(Usuario nuevoUsuario)
        {
            try
            {
                _context.Usuarios.Add(nuevoUsuario);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al insertar nuevo usuario");
                return Result.Failure<bool>(Error.Create("No se pudo guardar el usuario. Verifique los datos."));
            }
        }

        public Result<bool> UpdateUsuario(short id, Usuario usuarioActualizado)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);

                if (usuario == null)
                    return Result.Failure<bool>(Error.Create("Usuario no encontrado para actualizar"));

                usuario.Nombre = usuarioActualizado.Nombre;
                usuario.ApellidoPaterno = usuarioActualizado.ApellidoPaterno;
                usuario.ApellidoMaterno = usuarioActualizado.ApellidoMaterno;
                usuario.Telefono = usuarioActualizado.Telefono;
                usuario.Correo = usuarioActualizado.Correo;
                usuario.Estado = usuarioActualizado.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar usuario {id}", id);
                return Result.Failure<bool>(Error.Create("Error al actualizar los datos del usuario"));
            }
        }

        public Result<bool> DeleteUsuario(short id)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);

                if (usuario == null)
                    return Result.Failure<bool>(Error.Create("El usuario no existe"));

                // Borrado lógico: Estado 2
                usuario.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar usuario {id}", id);
                return Result.Failure<bool>(Error.Create("Error al intentar eliminar el usuario"));
            }
        }

        Result<List<Usuario>> IUsuarioDependencies.GetUsuarios()
        {
            throw new NotImplementedException();
        }

        Result<Usuario> IUsuarioDependencies.GetUsuarioById(short id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddUsuarios(Usuario nuevoUsuario)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateUsuarios(short id, Usuario usuarioActualizado)
        {
            throw new NotImplementedException();
        }
    }
}