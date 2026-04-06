using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;

namespace Backend.Service
{
    public interface ISeriesDependencies
    {
        Result<List<Serie>> GetSeries();
        Result<Serie> GetSerieById(string id);
        Result<bool> AddSerie(Serie nuevaSerie);
        Result<bool> UpdateSerie(string id, Serie serieActualizada);
        Result<bool> DeleteSerie(string id);
    }

    public class SerieService
    {
        private readonly ISeriesDependencies _dependencies;
        private readonly ILogger<SerieService> _log;

        public SerieService(ISeriesDependencies dependencies, ILogger<SerieService> logs)
        {
            _dependencies = dependencies;
            _log = logs;
        }

        public SerieService(ISeriesDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public Result<List<Serie>> GetSeries() => _dependencies.GetSeries();

        public Result<Serie> GetSerieById(string id) => _dependencies.GetSerieById(id);

        public Result<bool> DeleteSerie(string id) => _dependencies.DeleteSerie(id);

        public Result<bool> AddSerie(Serie nuevaSerie)
        {
            return ValidateSerie(nuevaSerie)
                .Bind(_dependencies.AddSerie);
        }

        public Result<bool> UpdateSerie(string id, Serie serieActualizada)
        {
            return GetSerieById(id)
                .Bind(_ => ValidateSerie(serieActualizada))
                .Bind(validSerie => _dependencies.UpdateSerie(id, validSerie));
        }

        private Result<Serie> ValidateSerie(Serie nuevaSerie)
        {
            _log.LogInformation("Agregando una nueva serie");

            List<Error> errores = new List<Error>();

            if (string.IsNullOrEmpty(nuevaSerie.Titulo))
                errores.Add(Error.Create("El Titulo no puede estar vacio"));

            if (string.IsNullOrEmpty(nuevaSerie.Director))
                errores.Add(Error.Create("El director no puede estar vacio"));

            if (nuevaSerie.Duracion == 0)
                errores.Add(Error.Create("La duracon no puede estar vacio"));

            if (string.IsNullOrEmpty(nuevaSerie.Poster))
                errores.Add(Error.Create("El Poster no puede estar vacio"));


            if (nuevaSerie.AnioEstreno == 0)
                errores.Add(Error.Create("El año de estreno no puede estar vacio"));

            if (string.IsNullOrEmpty(nuevaSerie.Plataforma))
                errores.Add(Error.Create("La plataforma no puede estar vacia"));

            if (errores.Any())
            {
                _log.LogWarning("Error al cargar la serie:{errores}", string.Join(", ", errores));
                return Result.Failure<Serie>(errores.ToImmutableArray());
            }

            return Result.Success(nuevaSerie);
        }
    }
}
