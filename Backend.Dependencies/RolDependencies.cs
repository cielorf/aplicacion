using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class RolDependencies : IRolDependencies
    {
        private readonly ILogger<RolDependencies> _log;
        private readonly BdenviusaContext _context;

        public RolDependencies(ILogger<RolDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Rol>> GetRoles()
        {
            try
            {
                var roles = _context.Rols.ToList();
                return Result.Success(roles);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener los roles de la base de datos");
                return Result.Failure<List<Rol>>(Error.Create("Error al obtener roles"));
            }
        }

        public Result<Rol> GetRolById(short id)
        {
            try
            {
                var rol = _context.Rols.FirstOrDefault(r => r.IdRol == id);

                if (rol == null)
                    return Result.Failure<Rol>(Error.Create("El rol no existe"));

                return Result.Success(rol);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al buscar el rol con ID {id}", id);
                return Result.Failure<Rol>(Error.Create("Error al buscar el rol"));
            }
        }

        public Result<bool> AddRol(Rol nuevoRol)
        {
            try
            {
                _context.Rols.Add(nuevoRol);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al insertar el nuevo rol");
                return Result.Failure<bool>(Error.Create("Error al guardar el rol en SQL Server"));
            }
        }

        public Result<bool> UpdateRol(short id, Rol rolActualizado)
        {
            try
            {
                var rol = _context.Rols.FirstOrDefault(r => r.IdRol == id);

                if (rol == null)
                    return Result.Failure<bool>(Error.Create("No se encontró el rol para actualizar"));

                rol.Nombre = rolActualizado.Nombre;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar el rol con ID {id}", id);
                return Result.Failure<bool>(Error.Create("Error al actualizar el rol"));
            }
        }

        public Result<bool> DeleteRol(short id)
        {
            try
            {
                var rol = _context.Rols.FirstOrDefault(r => r.IdRol == id);

                if (rol == null)
                    return Result.Failure<bool>(Error.Create("El rol no existe"));

                
                _context.Rols.Remove(rol);
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar el rol {id}", id);
                return Result.Failure<bool>(Error.Create("Error al intentar eliminar el rol"));
            }
        }
    }
}