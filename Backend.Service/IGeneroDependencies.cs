using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Backend.Service
{
    public interface IGeneroDependencies
    {
        Result<List<Genero>> GetGeneros();
        Result<Genero> GetGeneroById(string id);
        Result<bool> AddGenero(Genero nuevaGenero);
        Result<bool> UpdateGenero(string id, Genero generoActualizada);
        Result<bool> DeleteGenero(string id);
    }

    public class GeneroService
    {
        private readonly IGeneroDependencies _dependencies;
        private readonly ILogger<GeneroService> _log;

        public GeneroService(IGeneroDependencies dependencies, ILogger<GeneroService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Genero>> GetGeneros() => _dependencies.GetGeneros();

        public Result<Genero> GetGeneroById(string id) => _dependencies.GetGeneroById(id);

        public Result<bool> DeleteGenero(string id) => _dependencies.DeleteGenero(id);

        public Result<bool> AddGenero(Genero nuevoGenero)
        {
            return ValidateGenero(nuevoGenero)
                .Bind(_dependencies.AddGenero);
        }

        public Result<bool> UpdateGenero(string id, Genero generoActualizado)
        {
            return GetGeneroById(id)
                .Bind(_ => ValidateGenero(generoActualizado))
                .Bind(validGenero => _dependencies.UpdateGenero(id, validGenero));
        }

        private Result<Genero> ValidateGenero(Genero nuevoGenero)
        {
            _log.LogInformation("Agregando un nuevo Genero");

            List<Error> errores = new List<Error>();

            //Nombre
            if (string.IsNullOrEmpty(nuevoGenero.Nombre))
                errores.Add(Error.Create("El nombre no puede estar vacio"));

            if (errores.Any())
            {
                _log.LogWarning("Error al cargar el genero:{errores}", string.Join(", ", errores));
                return Result.Failure<Genero>(errores.ToImmutableArray());
            }

            return Result.Success(nuevoGenero);
        }
    }
}

