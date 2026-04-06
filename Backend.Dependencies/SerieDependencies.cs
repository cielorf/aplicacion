using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class SerieDependencies : ISeriesDependencies
    {
        private readonly ILogger<SerieDependencies> _log;
        private readonly BdenviusaContext _context;

        public SerieDependencies(ILogger<SerieDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Serie>> GetSeries()
        {
            try
            {
                var series = _context.Series
                    .Where(s => s.Estado == 1)
                    .ToList();

                return Result.Success(series);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener series");
                return Result.Failure<List<Serie>>(Error.Create("Error al obtener series"));
            }
        }

        public Result<Serie> GetSerieById(short id)
        {
            try
            {
                var serie = _context.Series.FirstOrDefault(s => s.IdSerie == id);

                if (serie == null)
                    return Result.Failure<Serie>(Error.Create("Serie no encontrada"));

                return Result.Success(serie);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener serie");
                return Result.Failure<Serie>(Error.Create("Error al obtener serie"));
            }
        }

        public Result<bool> AddSerie(Serie nuevaSerie)
        {
            try
            {
                _context.Series.Add(nuevaSerie);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar serie");
                return Result.Failure<bool>(Error.Create("Error al agregar serie"));
            }
        }

        public Result<bool> UpdateSerie(short id, Serie serieActualizada)
        {
            try
            {
                var serie = _context.Series.FirstOrDefault(s => s.IdSerie == id);

                if (serie == null)
                    return Result.Failure<bool>(Error.Create("Serie no encontrada"));

                // Mapeo de campos (ajusta los nombres si en tu base de datos son distintos)
                serie.Titulo = serieActualizada.Titulo;
                serie.Director = serieActualizada.Director;
                serie.Duracion = serieActualizada.Duracion;
                serie.Poster = serieActualizada.Poster;
                serie.AnioEstreno = serieActualizada.AnioEstreno;
                serie.Estado = serieActualizada.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar serie");
                return Result.Failure<bool>(Error.Create("Error al actualizar serie"));
            }
        }

        public Result<bool> DeleteSerie(short id)
        {
            try
            {
                var serie = _context.Series.FirstOrDefault(s => s.IdSerie == id);

                if (serie == null)
                    return Result.Failure<bool>(Error.Create("Serie no encontrada"));

                // Borrado lógico: cambiamos el estado a 2 (Inactivo/Eliminado)
                serie.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar serie");
                return Result.Failure<bool>(Error.Create("Error al eliminar serie"));
            }
        }

        public Result<Serie> GetSerieById(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateSerie(string id, Serie serieActualizada)
        {
            throw new NotImplementedException();
        }

        public Result<bool> DeleteSerie(string id)
        {
            throw new NotImplementedException();
        }
    }
}
