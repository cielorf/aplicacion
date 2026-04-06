using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class SesionDependencies : ISesionDependencies
    {
        private readonly ILogger<SesionDependencies> _log;
        private readonly BdenviusaContext _context;

        public SesionDependencies(ILogger<SesionDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Sesion>> GetSesiones()
        {
            try
            {
                var sesiones = _context.Sesions
                    .Where(s => s.Estado == 1)
                    .ToList();

                return Result.Success(sesiones);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener la lista de sesiones");
                return Result.Failure<List<Sesion>>(Error.Create("Error al obtener sesiones de la base de datos"));
            }
        }

        public Result<Sesion> GetSesionById(short id)
        {
            try
            {
                var sesion = _context.Sesions.FirstOrDefault(s => s.IdSesion == id);

                if (sesion == null)
                    return Result.Failure<Sesion>(Error.Create("Sesión no encontrada"));

                return Result.Success(sesion);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al buscar sesión con ID {id}", id);
                return Result.Failure<Sesion>(Error.Create("Error al buscar el registro de sesión"));
            }
        }

        public Result<bool> AddSesion(Sesion nuevaSesion)
        {
            try
            {
                _context.Sesions.Add(nuevaSesion);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al insertar nueva sesión");
                return Result.Failure<bool>(Error.Create("No se pudo registrar la sesión. Verifique los datos."));
            }
        }

        public Result<bool> UpdateSesion(short id, Sesion sesionActualizada)
        {
            try
            {
                var sesion = _context.Sesions.FirstOrDefault(s => s.IdSesion == id);

                if (sesion == null)
                    return Result.Failure<bool>(Error.Create("Sesión no encontrada para actualizar"));

                sesion.FechaHora = sesionActualizada.FechaHora;
                sesion.Estado = sesionActualizada.Estado;
                sesion.IdUsuario = sesionActualizada.IdUsuario;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar sesión {id}", id);
                return Result.Failure<bool>(Error.Create("Error al actualizar los datos de la sesión"));
            }
        }

        public Result<bool> DeleteSesion(short id)
        {
            try
            {
                var sesion = _context.Sesions.FirstOrDefault(s => s.IdSesion == id);

                if (sesion == null)
                    return Result.Failure<bool>(Error.Create("El registro de sesión no existe"));

                // Borrado lógico: Estado 2
                sesion.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar sesión {id}", id);
                return Result.Failure<bool>(Error.Create("Error al intentar realizar el borrado lógico de la sesión"));
            }
        }

     

        Result<List<Sesion>> ISesionDependencies.GetSesiones()
        {
            throw new NotImplementedException();
        }

        Result<Sesion> ISesionDependencies.GetSesionById(short id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddSesiones(Sesion nuevaSesion)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateSesiones(short id, Sesion sesionActualizada)
        {
            throw new NotImplementedException();
        }
    }
}