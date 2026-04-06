using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class TemporadaDependencies : ITemporadaDependencies
    {
        private readonly ILogger<TemporadaDependencies> _log;
        private readonly BdenviusaContext _context;

        public TemporadaDependencies(ILogger<TemporadaDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Temporadum>> GetTemporadas()
        {
            try
            {
                // Filtramos por estado activo (1)
                var temporadas = _context.Temporada
                    .Where(t => t.Estado == 1)
                    .ToList();

                return Result.Success(temporadas);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener las temporadas de la base de datos");
                return Result.Failure<List<Temporadum>>(Error.Create("Error al obtener temporadas"));
            }
        }

        public Result<Temporadum> GetTemporadaById(short id)
        {
            try
            {
                // Buscamos por la PK id_temporada
                var temporada = _context.Temporada.FirstOrDefault(t => t.IdTemporada == id);

                if (temporada == null)
                    return Result.Failure<Temporadum>(Error.Create("Temporada no encontrada"));

                return Result.Success(temporada);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener la temporada con ID {id}", id);
                return Result.Failure<Temporadum>(Error.Create("Error al buscar la temporada"));
            }
        }

        public Result<bool> AddTemporada(Temporadum nuevaTemporada)
        {
            try
            {
                _context.Temporada.Add(nuevaTemporada);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al insertar la nueva temporada");
                return Result.Failure<bool>(Error.Create("Error al guardar la temporada en la base de datos"));
            }
        }

        public Result<bool> UpdateTemporada(short id, Temporadum temporadaActualizada)
        {
            try
            {
                var temporada = _context.Temporada.FirstOrDefault(t => t.IdTemporada == id);

                if (temporada == null)
                    return Result.Failure<bool>(Error.Create("No se encontró la temporada para actualizar"));

                // Mapeo según los campos de tu diagrama
                temporada.NumeroTemporada = temporadaActualizada.NumeroTemporada;
                temporada.Episodios = temporadaActualizada.Episodios;
                temporada.Sinopsis = temporadaActualizada.Sinopsis;
                temporada.Estado = temporadaActualizada.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar la temporada con ID {id}", id);
                return Result.Failure<bool>(Error.Create("Error al actualizar los datos en SQL Server"));
            }
        }

        public Result<bool> DeleteTemporada(short id)
        {
            try
            {
                var temporada = _context.Temporada.FirstOrDefault(t => t.IdTemporada == id);

                if (temporada == null)
                    return Result.Failure<bool>(Error.Create("La temporada no existe"));

                // Borrado lógico: Estado 2 (Inactivo)
                temporada.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar (lógico) la temporada {id}", id);
                return Result.Failure<bool>(Error.Create("Error al intentar eliminar la temporada"));
            }
        }

        Result<List<Temporadum>> ITemporadaDependencies.GetTemporadas()
        {
            throw new NotImplementedException();
        }

        Result<Temporadum> ITemporadaDependencies.GetTemporadaById(short id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddTemporadas(Temporadum nuevaTemporada)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateTemporadas(short id, Temporadum temporadaActualizada)
        {
            throw new NotImplementedException();
        }
    }
}
