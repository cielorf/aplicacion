using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Backend.Service
{
    public interface ITemporadaDependencies
    {
        Result<List<Temporadum>> GetTemporadas();
        Result<Temporadum> GetTemporadaById(short id);
        Result<bool> AddTemporada(Temporadum nuevaTemporada);
        Result<bool> UpdateTemporada(short id, Temporadum temporadaActualizada);
        Result<bool> DeleteTemporada(short id);
    }

    public class TemporadaService
    {
        private readonly ITemporadaDependencies _dependencies;
        private readonly ILogger<TemporadaService> _log;

        public TemporadaService(ITemporadaDependencies dependencies, ILogger<TemporadaService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Temporadum>> GetTemporadas() => _dependencies.GetTemporadas();

        public Result<Temporadum> GetTemporadaById(short id) => _dependencies.GetTemporadaById(id);

        public Result<bool> DeleteTemporada(short id) => _dependencies.DeleteTemporada(id);

        public Result<bool> AddTemporada(Temporadum nuevaTemporada)
        {
            return ValidateTemporada(nuevaTemporada)
                .Bind(_dependencies.AddTemporada);
        }

        public Result<bool> UpdateTemporada(short id, Temporadum temporadaActualizada)
        {
            return GetTemporadaById(id)
                .Bind(_ => ValidateTemporada(temporadaActualizada))
                .Bind(validTemp => _dependencies.UpdateTemporada(id, validTemp));
        }

        private Result<Temporadum> ValidateTemporada(Temporadum temporada)
        {
            _log.LogInformation("Validando datos de la Temporada");

            List<Error> errores = new List<Error>();

            // Validación de Número de Temporada (tinyint en diagrama)
            if (temporada.NumeroTemporada <= 0)
                errores.Add(Error.Create("El número de temporada debe ser mayor a 0"));

            // Validación de Episodios (smallint en diagrama)
            if (temporada.Episodios <= 0)
                errores.Add(Error.Create("La temporada debe tener al menos 1 episodio"));

            // Validación de Sinopsis (varchar 100 en diagrama)
            if (string.IsNullOrWhiteSpace(temporada.Sinopsis))
                errores.Add(Error.Create("La sinopsis es obligatoria"));

            if (temporada.Sinopsis?.Length > 100)
                errores.Add(Error.Create("La sinopsis no puede exceder los 100 caracteres"));

            if (errores.Any())
            {
                _log.LogWarning("Errores de validación en Temporada: {errores}", string.Join(", ", errores.Select(e => e.Message)));
                return Result.Failure<Temporadum>(errores.ToImmutableArray());
            }

            return Result.Success(temporada);
        }
    }
}