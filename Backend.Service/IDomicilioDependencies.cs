using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;

namespace Backend.Service
{
    public interface IDomicilioDependencies
    {
        Result<List<Domicilio>> GetDomicilios();
        Result<Domicilio> GetDomicilioById(short id);
        Result<bool> AddDomicilio(Domicilio nuevoDomicilio);
        Result<bool> UpdateDomicilio(short id, Domicilio domicilioActualizado);
        Result<bool> DeleteDomicilio(short id);
    }

    public class DomicilioService
    {
        private readonly IDomicilioDependencies _dependencies;
        private readonly ILogger<DomicilioService> _log;

        public DomicilioService(IDomicilioDependencies dependencies, ILogger<DomicilioService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Domicilio>> GetDomicilios() => _dependencies.GetDomicilios();

        public Result<Domicilio> GetDomicilioById(short id) => _dependencies.GetDomicilioById(id);

        public Result<bool> DeleteDomicilio(short id) => _dependencies.DeleteDomicilio(id);

        public Result<bool> AddDomicilio(Domicilio nuevoDomicilio)
        {
            return ValidateDomicilio(nuevoDomicilio)
                .Bind(_dependencies.AddDomicilio);
        }

        public Result<bool> UpdateDomicilio(short id, Domicilio domicilioActualizado)
        {
            return GetDomicilioById(id)
                .Bind(_ => ValidateDomicilio(domicilioActualizado))
                .Bind(validDom => _dependencies.UpdateDomicilio(id, validDom));
        }

        private Result<Domicilio> ValidateDomicilio(Domicilio domicilio)
        {
            _log.LogInformation("Validando datos del Domicilio para el Usuario ID: {idUsuario}", domicilio.IdUsuario);

            List<Error> errores = new List<Error>();

            // Validación de Calle
            if (string.IsNullOrWhiteSpace(domicilio.Calle))
                errores.Add(Error.Create("La calle es obligatoria"));

            // Validación de Número Exterior
            if (string.IsNullOrWhiteSpace(domicilio.NumeroExterior))
                errores.Add(Error.Create("El número exterior no puede estar vacío"));

            // Validación de Colonia
            if (string.IsNullOrWhiteSpace(domicilio.Colonia))
                errores.Add(Error.Create("La colonia es obligatoria"));

            // Validación de Ciudad
            if (string.IsNullOrWhiteSpace(domicilio.Ciudad))
                errores.Add(Error.Create("La ciudad es obligatoria"));

            // Validación de ID Usuario (Relación obligatoria)
            if (domicilio.IdUsuario <= 0)
                errores.Add(Error.Create("El domicilio debe estar vinculado a un Usuario válido"));

            if (errores.Any())
            {
                _log.LogWarning("Errores de validación en Domicilio: {errores}", string.Join(", ", errores.Select(e => e.Message)));
                return Result.Failure<Domicilio>(errores.ToImmutableArray());
            }

            return Result.Success(domicilio);
        }
    }
}