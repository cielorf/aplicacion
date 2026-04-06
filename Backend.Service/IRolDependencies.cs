using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Backend.Service
{
    public interface IRolDependencies
    {
        Result<List<Rol>> GetRoles();
        Result<Rol> GetRolById(short id);
        Result<bool> AddRol(Rol nuevoRol);
        Result<bool> UpdateRol(short id, Rol rolActualizado);
        Result<bool> DeleteRol(short id);
    }

    public class RolService
    {
        private readonly IRolDependencies _dependencies;
        private readonly ILogger<RolService> _log;

        public RolService(IRolDependencies dependencies, ILogger<RolService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Rol>> GetRoles() => _dependencies.GetRoles();

        public Result<Rol> GetRolById(short id) => _dependencies.GetRolById(id);

        public Result<bool> DeleteRol(short id) => _dependencies.DeleteRol(id);

        public Result<bool> AddRol(Rol nuevoRol)
        {
            return ValidateRol(nuevoRol)
                .Bind(_dependencies.AddRol);
        }

        public Result<bool> UpdateRol(short id, Rol rolActualizado)
        {
            return GetRolById(id)
                .Bind(_ => ValidateRol(rolActualizado))
                .Bind(validRol => _dependencies.UpdateRol(id, validRol));
        }

        private Result<Rol> ValidateRol(Rol nuevoRol)
        {
            _log.LogInformation("Validando el nuevo Rol: {nombre}", nuevoRol.Nombre);

            List<Error> errores = new List<Error>();

            // Validación de Nombre
            if (string.IsNullOrWhiteSpace(nuevoRol.Nombre))
                errores.Add(Error.Create("El nombre del rol no puede estar vacío"));

            // Validación de longitud (según tu diagrama varchar(50))
            if (nuevoRol.Nombre?.Length > 50)
                errores.Add(Error.Create("El nombre del rol no puede exceder los 50 caracteres"));

            if (errores.Any())
            {
                _log.LogWarning("Error al validar el rol: {errores}", string.Join(", ", errores.Select(e => e.Message)));
                return Result.Failure<Rol>(errores.ToImmutableArray());
            }

            return Result.Success(nuevoRol);
        }
    }
}