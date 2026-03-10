using Backend.Data.Models;
using Backend.Service;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ROP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Dependencies
{
    public class PeliculasDependencies : IPeliculasDependencies
    {
        private readonly ILogger<PeliculasDependencies> _log;
        private readonly MongoDbContext _context;

        public PeliculasDependencies(ILogger<PeliculasDependencies> log, MongoDbContext context)
        {
            _log = log;
            _context = context;
        }

        public Result<bool> AddPelicula(Pelicula nuevaPelicula)
        {
            try
            {
                _context.Peliculas.InsertOne(nuevaPelicula);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar pelicula");
                return Result.Failure<bool>(Error.Create("Error al agregar pelicula"));
            }
        }

        public Result<List<Pelicula>> GetPeliculas()
        {
            try
            {
                var peliculas = _context.Peliculas.Find(_ => true).ToList();
                return Result.Success(peliculas);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener peliculas");
                return Result.Failure<List<Pelicula>>(Error.Create("Error al obtener peliculas"));
            }
        }

        public Result<Pelicula> GetPeliculaById(string id)
        {
            try
            {
                var filter = Builders<Pelicula>.Filter.Eq(p => p.Id, id);
                var pelicula = _context.Peliculas.Find(filter).FirstOrDefault();

                if (pelicula != null)
                    return Result.Success(pelicula);

                return Result.Failure<Pelicula>(Error.Create("Pelicula no encontrada"));
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al obtener pelicula");
                return Result.Failure<Pelicula>(Error.Create("Error al obtener pelicula"));
            }
        }

        public Result<bool> DeletePelicula(string id)
        {
            try
            {
                var filter = Builders<Pelicula>.Filter.Eq(p => p.Id, id);
                var result = _context.Peliculas.DeleteOne(filter);

                if (result.DeletedCount > 0)
                    return Result.Success(true);

                return Result.Failure<bool>(Error.Create("Pelicula no encontrada"));
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al eliminar pelicula");
                return Result.Failure<bool>(Error.Create("Error al eliminar pelicula"));
            }
        }

        public Result<bool> UpdatePelicula(string id, Pelicula peliculaActualizada)
        {
            try
            {
                var filter = Builders<Pelicula>.Filter.Eq(p => p.Id, id);
                var result = _context.Peliculas.ReplaceOne(filter, peliculaActualizada);

                if (result.ModifiedCount > 0)
                    return Result.Success(true);

                return Result.Failure<bool>(Error.Create("Pelicula no encontrada"));
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al actualizar pelicula");
                return Result.Failure<bool>(Error.Create("Error al actualizar pelicula"));
            }
        }
    }
}
