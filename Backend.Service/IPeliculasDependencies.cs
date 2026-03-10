using Backend.Data.Models;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;

namespace Backend.Service
{
    public interface IPeliculasDependencies
    {
        Result<List<Pelicula>> GetPeliculas();

        Result<Pelicula> GetPeliculaById(string id);

        Result<bool> AddPelicula(Pelicula nuevaPelicula);

        Result<bool> UpdatePelicula(string id, Pelicula peliculaActualizada);

        Result<bool> DeletePelicula(string id);
    }

    public class PeliculasService
    {
        private readonly IPeliculasDependencies _dependencies;
        private readonly ILogger<PeliculasService> _log;

        public PeliculasService(IPeliculasDependencies dependencies, ILogger<PeliculasService> log)
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

            // Genero
            if (string.IsNullOrEmpty(nuevaPelicula.Genero))
                errores.Add(Error.Create("El genero no puede estar vacio"));

            // Duración
            if (nuevaPelicula.DuracionMinutos <= 0)
                errores.Add(Error.Create("La duración debe ser mayor a 0"));

            // Fecha estreno
            if (nuevaPelicula.FechaEstreno > DateOnly.FromDateTime(DateTime.Now))
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