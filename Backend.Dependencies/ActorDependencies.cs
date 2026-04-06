using Backend.Data.Models.MSSQL;
using Backend.Service;
using Microsoft.Extensions.Logging;
using ROP;

namespace Backend.Dependencies
{
    public class ActorDependencies : IActorDependencies
    {
        private readonly ILogger<ActorDependencies> _log;
        private readonly BdenviusaContext _context;

        public ActorDependencies(ILogger<ActorDependencies> log, BdenviusaContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<List<Actor>> GetActores()
        {
            try
            {
                var actores = _context.Actors
                    .Where(a => a.Estado == 1)
                    .ToList();

                return Result.Success(actores);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener actores");
                return Result.Failure<List<Actor>>(Error.Create("Error al obtener actores"));
            }
        }

        public Result<Actor> GetActorById(short id)
        {
            try
            {
                var actor = _context.Actors.FirstOrDefault(a => a.IdActor == id);

                if (actor == null)
                    return Result.Failure<Actor>(Error.Create("Actor no encontrado"));

                return Result.Success(actor);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener actor");
                return Result.Failure<Actor>(Error.Create("Error al obtener actor"));
            }
        }

        public Result<bool> AddActor(Actor nuevoActor)
        {
            try
            {
                _context.Actors.Add(nuevoActor);
                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar actor");
                return Result.Failure<bool>(Error.Create("Error al agregar actor"));
            }
        }

        public Result<bool> UpdateActor(short id, Actor actorActualizado)
        {
            try
            {
                var actor = _context.Actors.FirstOrDefault(a => a.IdActor == id);

                if (actor == null)
                    return Result.Failure<bool>(Error.Create("Actor no encontrado"));

                // Mapeo completo según tu diagrama relacional
                actor.Nombre = actorActualizado.Nombre;
                actor.ApellidoPaterno = actorActualizado.ApellidoPaterno;
                actor.ApellidoMaterno = actorActualizado.ApellidoMaterno;
                actor.FechaNac = actorActualizado.FechaNac;
                actor.Poster = actorActualizado.Poster;
                actor.Estado = actorActualizado.Estado;

                _context.SaveChanges();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar actor");
                return Result.Failure<bool>(Error.Create("Error al actualizar actor"));
            }
        }

        public Result<bool> DeleteActor(short id)
        {
            try
            {
                var actor = _context.Actors.FirstOrDefault(a => a.IdActor == id);

                if (actor == null)
                    return Result.Failure<bool>(Error.Create("Actor no encontrado"));

                // Borrado lógico
                actor.Estado = 2;
                _context.SaveChanges();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar actor");
                return Result.Failure<bool>(Error.Create("Error al eliminar actor"));
            }
        }

       
        public Result<Actor> GetActorById(string id) => throw new NotImplementedException();
        public Result<bool> UpdateActor(string id, Actor actorActualizada) => throw new NotImplementedException();
        public Result<bool> DeleteActor(string id) => throw new NotImplementedException();
    }
}