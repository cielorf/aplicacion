using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;

namespace Backend.Service
{
    public interface ISesionDependencies
    {
        Result<List<Sesion>> GetSesiones();
        Result<Sesion> GetSesionById(short id);
        Result<bool> AddSesion(Sesion nuevaSesion);
        Result<bool> DeleteSesion(short id);
        Result<bool> UpdateSesion(short id, Sesion sesionActualizada);
    }

    public class SesionService
    {
        private readonly ISesionDependencies _dependencies;
        private readonly ILogger<SesionService> _log;

        public SesionService(ISesionDependencies dependencies, ILogger<SesionService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Sesion>> GetSesiones() => _dependencies.GetSesiones();

        public Result<Sesion> GetSesionById(short id) => _dependencies.GetSesionById(id);

        public Result<bool> DeleteSesion(short id) => _dependencies.DeleteSesion(id);

        public Result<bool> AddSesion(Sesion nuevaSesion)
        {
            return ValidateSesion(nuevaSesion)
                .Bind(_dependencies.AddSesion);
        }

        public Result<bool> UpdateSesion(short id, Sesion sesionActualizada)
        {
            return GetSesionById(id)
                .Bind(_ => ValidateSesion(sesionActualizada))
                .Bind(validSesion => _dependencies.UpdateSesion(id, validSesion));
        }

        private Result<Sesion> ValidateSesion(Sesion sesion)
        {
            _log.LogInformation("Validando datos de la Sesión para el Usuario ID: {idUsuario}", sesion.IdUsuario);

            List<Error> errores = new List<Error>();

            // Validación de ID de Usuario (FK obligatoria)
            if (sesion.IdUsuario <= 0)
                errores.Add(Error.Create("La sesión debe estar vinculada a un Usuario válido"));

            if (string.IsNullOrWhiteSpace(sesion.Nickname))
                errores.Add(Error.Create("El nickname es obligatorio para la sesión"));

            if (string.IsNullOrWhiteSpace(sesion.Contraseña))
                errores.Add(Error.Create("La contraseña no puede estar vacía"));

            if (errores.Any())
            {
                _log.LogWarning("Errores de validación en Sesion: {errores}", string.Join(", ", errores.Select(e => e.Message)));
                return Result.Failure<Sesion>(errores.ToImmutableArray());
            }

            return Result.Success(sesion);
        }
    }
}