using Backend.Data.Models;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;
using System.Linq;

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

    /// <summary>
    /// Servicio de Series que maneja la lógica de negocio y validaciones.
    /// Delegando las operaciones de datos a través de ISeriesDependencies.
    /// </summary>
    public class SeriesService
    {
        private readonly ISeriesDependencies _dependencies;
        private readonly ILogger<SeriesService> _log;

        public SeriesService(ISeriesDependencies dependencies, ILogger<SeriesService> logs)
        {
            _dependencies = dependencies;
            _log = logs;
        }

        public SeriesService(ISeriesDependencies dependencies)
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

            if (string.IsNullOrEmpty(nuevaSerie.Plataforma))
                errores.Add(Error.Create("La Plataforma no puede estar vacia"));

            if (nuevaSerie.AnioEstreno == 0)
                errores.Add(Error.Create("El Año de estreno no puede estar vacio"));

            if (string.IsNullOrEmpty(nuevaSerie.Genero))
                errores.Add(Error.Create("El Genero no puede estar vacio"));

            if (nuevaSerie.TemporadasEpisodios == null ||
                !nuevaSerie.TemporadasEpisodios.Any())
                errores.Add(Error.Create("Debe tener al menos una temporada"));

            if (nuevaSerie.TemporadasEpisodios != null)
            {
                foreach (var temp in nuevaSerie.TemporadasEpisodios)
                {
                    if (temp.Temporadas <= 0)
                        errores.Add(Error.Create("La temporada no puede ser 0"));

                    if (temp.Episodios <= 0)
                        errores.Add(Error.Create("El episodio no puede ser 0"));
                }
            }

            if (errores.Any())
            {
                _log.LogWarning("Error al cargar la serie:{errores}", string.Join(", ", errores));
                return Result.Failure<Serie>(errores.ToImmutableArray());
            }

            return Result.Success(nuevaSerie);
        }
    }
}