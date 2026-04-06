using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class DomicilioDependencies : IDomicilioDependencies
    {
        private readonly ILogger<DomicilioDependencies> _log;
        private readonly BdenviusaContext _context;

        public DomicilioDependencies(ILogger<DomicilioDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Domicilio>> GetDomicilios()
        {
            try
            {
                var domicilios = _context.Domicilios
                    .Where(d => d.Estado == 1)
                    .ToList();

                return Result.Success(domicilios);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener domicilios");
                return Result.Failure<List<Domicilio>>(Error.Create("Error al obtener domicilios"));
            }
        }

        public Result<Domicilio> GetDomicilioById(short id)
        {
            try
            {
                var domicilio = _context.Domicilios.FirstOrDefault(d => d.IdDomicilio == id);

                if (domicilio == null)
                    return Result.Failure<Domicilio>(Error.Create("Domicilio no encontrado"));

                return Result.Success(domicilio);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener domicilio por ID");
                return Result.Failure<Domicilio>(Error.Create("Error al obtener domicilio"));
            }
        }

        public Result<bool> AddDomicilio(Domicilio nuevoDomicilio)
        {
            try
            {
                _context.Domicilios.Add(nuevoDomicilio);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar domicilio");
                return Result.Failure<bool>(Error.Create("Error al agregar domicilio. Verifique que el usuario no tenga uno ya asignado."));
            }
        }

        public Result<bool> UpdateDomicilio(short id, Domicilio domicilioActualizado)
        {
            try
            {
                var domicilio = _context.Domicilios.FirstOrDefault(d => d.IdDomicilio == id);

                if (domicilio == null)
                    return Result.Failure<bool>(Error.Create("Domicilio no encontrado para actualizar"));

                domicilio.Calle = domicilioActualizado.Calle;
                domicilio.NumeroExterior = domicilioActualizado.NumeroExterior;
                domicilio.NumeroInterior = domicilioActualizado.NumeroInterior;
                domicilio.Colonia = domicilioActualizado.Colonia;
                domicilio.Ciudad = domicilioActualizado.Ciudad;
                domicilio.Referencias = domicilioActualizado.Referencias;
                domicilio.Estado = domicilioActualizado.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar domicilio");
                return Result.Failure<bool>(Error.Create("Error al actualizar domicilio"));
            }
        }

        public Result<bool> DeleteDomicilio(short id)
        {
            try
            {
                var domicilio = _context.Domicilios.FirstOrDefault(d => d.IdDomicilio == id);

                if (domicilio == null)
                    return Result.Failure<bool>(Error.Create("Domicilio no encontrado"));

                // Borrado lógico (Estado = 2)
                domicilio.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar domicilio");
                return Result.Failure<bool>(Error.Create("Error al eliminar domicilio"));
            }
        }

  
        public Result<Domicilio> GetDomicilioById(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateDomicilio(string id, Domicilio domicilioActualizado)
        {
            throw new NotImplementedException();
        }

        public Result<bool> DeleteDomicilio(string id)
        {
            throw new NotImplementedException();
        }
    }
}