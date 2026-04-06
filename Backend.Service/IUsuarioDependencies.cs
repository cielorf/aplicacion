using Backend.Data.Models.MSSQL;
using Microsoft.Extensions.Logging;
using ROP;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Backend.Service
{
    public interface IUsuarioDependencies
    {
        Result<List<Usuario>> GetUsuarios();
        Result<Usuario> GetUsuarioById(short id);
        Result<bool> AddUsuario(Usuario nuevoUsuario);
        Result<bool> UpdateUsuario(short id, Usuario usuarioActualizado);
        Result<bool> DeleteUsuario(short id);
    }

    public class UsuarioService
    {
        private readonly IUsuarioDependencies _dependencies;
        private readonly ILogger<UsuarioService> _log;

        public UsuarioService(IUsuarioDependencies dependencies, ILogger<UsuarioService> log)
        {
            _dependencies = dependencies;
            _log = log;
        }

        public Result<List<Usuario>> GetUsuarios() => _dependencies.GetUsuarios();

        public Result<Usuario> GetUsuarioById(short id) => _dependencies.GetUsuarioById(id);

        public Result<bool> DeleteUsuario(short id) => _dependencies.DeleteUsuario(id);

        public Result<bool> AddUsuario(Usuario nuevoUsuario)
        {
            return ValidateUsuario(nuevoUsuario)
                .Bind(_dependencies.AddUsuario);
        }

        public Result<bool> UpdateUsuario(short id, Usuario usuarioActualizado)
        {
            return GetUsuarioById(id)
                .Bind(_ => ValidateUsuario(usuarioActualizado))
                .Bind(validUser => _dependencies.UpdateUsuario(id, validUser));
        }

        private Result<Usuario> ValidateUsuario(Usuario usuario)
        {
            _log.LogInformation("Validando datos del Usuario: {correo}", usuario.Correo);

            List<Error> errores = new List<Error>();

            // Validación de Nombres y Apellidos
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                errores.Add(Error.Create("El nombre es obligatorio"));

            if (string.IsNullOrWhiteSpace(usuario.ApellidoPaterno))
                errores.Add(Error.Create("El apellido paterno es obligatorio"));

            // Validación de Correo (Formato)
            if (string.IsNullOrWhiteSpace(usuario.Correo) || !Regex.IsMatch(usuario.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.Add(Error.Create("El formato del correo electrónico no es válido"));

            // Validación de Teléfono (Exactamente 10 dígitos según tu diagrama varchar(10))
            if (!string.IsNullOrWhiteSpace(usuario.Telefono) && usuario.Telefono.Length != 10)
                errores.Add(Error.Create("El teléfono debe tener exactamente 10 dígitos"));

            if (errores.Any())
            {
                _log.LogWarning("Errores de validación en Usuario: {errores}", string.Join(", ", errores.Select(e => e.Message)));
                return Result.Failure<Usuario>(errores.ToImmutableArray());
            }

            return Result.Success(usuario);
        }
    }
}