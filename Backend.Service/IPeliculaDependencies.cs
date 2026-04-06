using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;


namespace Backend.Service
{
    public interface IPeliculaDependencies
    {
        Result<List<Pelicula>> GetPeliculas();
        Result<Pelicula> GetPeliculaById(string id);
        Result<bool> AddPelicula(Pelicula nuevaPelicula);
        Result<bool> UpdatePelicula(string id, Pelicula peliculaActualizada);
        Result<bool> DeletePelicula(string id);
    }

    public class PeliculaService
    {
        private readonly IPeliculaDependencies _dependencies;
        private readonly ILogger<PeliculasService> _log;

        public PeliculaService(IPeliculaDependencies dependencies, ILogger<PeliculasService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Pelicula>> GetPeliculas() => _dependencies.GetPeliculas();

        public Result<Pelicula> GetPeliculaById(string id) => _dependencies.GetPeliculaById(id);

        public Result<bool> DeletePelicula(string id) => _dependencies.DeletePelicula(id);

        public Result<bool> AddPelicula(Pelicula nuevaPelicula)
        {
            return ValidatePelicula(nuevaPelicula)
                .Bind(_dependencies.AddPelicula);
        }

        public Result<bool> UpdatePelicula(string id, Pelicula peliculaActualizada)
        {
            return GetPeliculaById(id)
                .Bind(_ => ValidatePelicula(peliculaActualizada))
                .Bind(validPelicula => _dependencies.UpdatePelicula(id, validPelicula));
        }

        private Result<Pelicula> ValidatePelicula(Pelicula nuevaPelicula)
        {
            _log.LogInformation("Agregando una nueva pelicula");

            List<Error> errores = new List<Error>();

            // Titulo
            if (string.IsNullOrEmpty(nuevaPelicula.Titulo))
                errores.Add(Error.Create("El titulo no puede estar vacio"));

            // Director
            if (string.IsNullOrEmpty(nuevaPelicula.Director))
                errores.Add(Error.Create("El director no puede estar vacio"));

            // Duración
            if (nuevaPelicula.Duracion <= 0)
                errores.Add(Error.Create("La duración debe ser mayor a 0"));

            // Resumen
            if (string.IsNullOrEmpty(nuevaPelicula.Resumen))
                errores.Add(Error.Create("El genero no puede estar vacio"));

            // Poster
            if (string.IsNullOrEmpty(nuevaPelicula.Poster))
                errores.Add(Error.Create("El poster no puede estar vacio"));

            // Fecha estreno
            if (nuevaPelicula.AnioEstreno > DateTime.Now.Year)
                errores.Add(Error.Create("La fecha de estreno no puede ser mayor a hoy"));

            if (errores.Any())
            {
                _log.LogWarning("Error al cargar la pelicula:{errores}", string.Join(", ", errores));
                return Result.Failure<Pelicula>(errores.ToImmutableArray());
            }

            return Result.Success(nuevaPelicula);
        }
    }
}