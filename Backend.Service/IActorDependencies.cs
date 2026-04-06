using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System.Collections.Immutable;


namespace Backend.Service
{
    public interface IActorDependencies
    {
        Result<List<Actor>> GetActores();
        Result<Actor> GetActorById(short id);
        Result<bool> AddActor(Actor nuevoActor);
        Result<bool> UpdateActor(short id, Actor actorActualizado);
        Result<bool> DeleteActor(short id);
    }

    public class ActorService
    {
        private readonly IActorDependencies _dependencies;
        private readonly ILogger<ActorService> _log;

        public ActorService(IActorDependencies dependencies, ILogger<ActorService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Actor>> GetActores() => _dependencies.GetActores();

        public Result<Actor> GetActorById(short id) => _dependencies.GetActorById(id);

        public Result<bool> DeleteActor(short id) => _dependencies.DeleteActor(id);

        public Result<bool> AddActor(Actor nuevoActor)
        {
            return ValidateActor(nuevoActor)
                .Bind(_dependencies.AddActor);
        }

        public Result<bool> UpdateActor(short id, Actor actorActualizado)
        {
            return GetActorById(id)
                .Bind(_ => ValidateActor(actorActualizado))
                .Bind(validActor => _dependencies.UpdateActor(id, validActor));
        }

        private Result<Actor> ValidateActor(Actor actor)
        {
            _log.LogInformation("Validando datos del Actor");

            List<Error> errores = new List<Error>();

            // Validación de Nombre
            if (string.IsNullOrWhiteSpace(actor.Nombre))
                errores.Add(Error.Create("El nombre del actor no puede estar vacío"));

            // Validación de Apellido Paterno
            if (string.IsNullOrWhiteSpace(actor.ApellidoPaterno))
                errores.Add(Error.Create("El apellido paterno no puede estar vacío"));

            if (actor.FechaNac > DateOnly.FromDateTime(DateTime.Now))
                errores.Add(Error.Create("La fecha de nacimiento no puede ser en el futuro"));
            

            if (errores.Any())
            {
                _log.LogWarning("Errores de validación en Actor: {errores}", string.Join(", ", errores.Select(e => e.Message)));
                return Result.Failure<Actor>(errores.ToImmutableArray());
            }

            return Result.Success(actor);
        }
    }
}
