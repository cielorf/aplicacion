using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class PeliculaDependencies : IPeliculaDependencies
    {
        private readonly ILogger<PeliculaDependencies> _log;
        private readonly BdenviusaContext _context;

        public PeliculaDependencies(ILogger<PeliculaDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Pelicula>> GetPeliculas()
        {
            try
            {
                var peliculas = _context.Peliculas
                    .Where(p => p.Estado == 1)
                    .ToList();

                return Result.Success(peliculas);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener peliculas");
                return Result.Failure<List<Pelicula>>(Error.Create("Error al obtener peliculas"));
            }
        }

        public Result<Pelicula> GetPeliculaById(short id)
        {
            try
            {
                var pelicula = _context.Peliculas.FirstOrDefault(p => p.IdPelicula == id);

                if (pelicula == null)
                    return Result.Failure<Pelicula>(Error.Create("Pelicula no encontrada"));

                return Result.Success(pelicula);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener pelicula");
                return Result.Failure<Pelicula>(Error.Create("Error al obtener pelicula"));
            }
        }

        public Result<bool> AddPelicula(Pelicula nuevaPelicula)
        {
            try
            {
                _context.Peliculas.Add(nuevaPelicula);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar pelicula");
                return Result.Failure<bool>(Error.Create("Error al agregar pelicula"));
            }
            
        }

        public Result<bool> UpdatePelicula(short id, Pelicula peliculaActualizada)
        {
            try
            {
                var pelicula = _context.Peliculas.FirstOrDefault(p => p.IdPelicula == id);

                if (pelicula == null)
                    return Result.Failure<bool>(Error.Create("Pelicula no encontrada"));

                pelicula.Titulo = peliculaActualizada.Titulo;
                pelicula.Director = peliculaActualizada.Director;
                pelicula.Duracion = peliculaActualizada.Duracion;
                pelicula.Resumen = peliculaActualizada.Resumen;
                pelicula.Poster = peliculaActualizada.Poster;
                pelicula.PrecioRecaudacion = peliculaActualizada.PrecioRecaudacion;
                pelicula.AnioEstreno = peliculaActualizada.AnioEstreno;
                pelicula.Estado = peliculaActualizada.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar pelicula");
                return Result.Failure<bool>(Error.Create("Error al actualizar pelicula"));
            }
        }

        public Result<bool> DeletePelicula(short id)
        {
            try
            {
                var pelicula = _context.Peliculas.FirstOrDefault(p => p.IdPelicula == id);

                if (pelicula == null)
                    return Result.Failure<bool>(Error.Create("Pelicula no encontrada"));

                // Borrado lógico
                pelicula.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar pelicula");
                return Result.Failure<bool>(Error.Create("Error al eliminar pelicula"));
            }
        }

        public Result<Pelicula> GetPeliculaById(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdatePelicula(string id, Pelicula peliculaActualizada)
        {
            throw new NotImplementedException();
        }

        public Result<bool> DeletePelicula(string id)
        {
            throw new NotImplementedException();
        }
    }
}
