using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class GeneroDependencies : IGeneroDependencies
    {
        private readonly ILogger<GeneroDependencies> _log;
        private readonly BdenviusaContext _context;

        public GeneroDependencies(ILogger<GeneroDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Genero>> GetGeneros()
        {
            try
            {
                var generos = _context.Generos
                    .Where(g => g.Estado == 1)
                    .ToList();

                return Result.Success(generos);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener generos");
                return Result.Failure<List<Genero>>(Error.Create("Error al obtener generos"));
            }
        }

        public Result<Genero> GetGeneroById(short id)
        {
            try
            {
                // Verifica si en tu tabla el ID se llama 'IdGenero'
                var genero = _context.Generos.FirstOrDefault(g => g.IdGenero == id);

                if (genero == null)
                    return Result.Failure<Genero>(Error.Create("Genero no encontrado"));

                return Result.Success(genero);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener genero");
                return Result.Failure<Genero>(Error.Create("Error al obtener genero"));
            }
        }

        public Result<bool> AddGenero(Genero nuevoGenero)
        {
            try
            {
                _context.Generos.Add(nuevoGenero);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar genero");
                return Result.Failure<bool>(Error.Create("Error al agregar genero"));
            }
        }

        public Result<bool> UpdateGenero(short id, Genero generoActualizado)
        {
            try
            {
                var genero = _context.Generos.FirstOrDefault(g => g.IdGenero == id);

                if (genero == null)
                    return Result.Failure<bool>(Error.Create("Genero no encontrado"));

                genero.Nombre = generoActualizado.Nombre;
                genero.Estado = generoActualizado.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar genero");
                return Result.Failure<bool>(Error.Create("Error al actualizar genero"));
            }
        }

        public Result<bool> DeleteGenero(short id)
        {
            try
            {
                var genero = _context.Generos.FirstOrDefault(g => g.IdGenero == id);

                if (genero == null)
                    return Result.Failure<bool>(Error.Create("Genero no encontrado"));

                // Borrado lógico (Estado = 2 suele ser inactivo)
                genero.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar genero");
                return Result.Failure<bool>(Error.Create("Error al eliminar genero"));
            }
        }


        public Result<Genero> GetGeneroById(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateGenero(string id, Pelicula generoActualizado)
        {
            throw new NotImplementedException();
        }

        public Result<bool> DeleteGenero(string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> UpdateGenero(string id, Genero generoActualizada)
        {
            throw new NotImplementedException();
        }
    }
}
