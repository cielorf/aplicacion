/*
using Backend.Data.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeliculasControllerMongo : ControllerBase
    {
        private readonly ILogger<PeliculasControllerMongo> _log;
        private readonly PeliculasService _peliculasService;

        public PeliculasControllerMongo(PeliculasService peliculasService, ILogger<PeliculasControllerMongo> log)
        {
            _peliculasService = peliculasService;
            _log = log;
        }
        
        // Ver todas
        [HttpGet("VerTodosMongo")]
        public IActionResult GetTodos()
        {
            _log.LogInformation("Obteniendo todas las peliculas");
            return _peliculasService.GetPeliculas().ToActionResult();
        }

        // Agregar
        [HttpPost("AgregarMongo")]
        public IActionResult Add(Pelicula nuevaPelicula)
        {
            _log.LogInformation("Agregando una nueva pelicula");
            return _peliculasService.AddPelicula(nuevaPelicula).ToActionResult();
        }

        // Borrar
        [HttpDelete("BorrarMongo/{id}")]
        public IActionResult Delete(string id)
        {
            _log.LogInformation("Eliminando la pelicula con ID: {Id}", id);
            return _peliculasService.DeletePelicula(id).ToActionResult();
        }

        // Actualizar
        [HttpPut("ActualizarMongo/{id}")]
        public IActionResult Update(string id, Pelicula peliculaActualizada)
        {
            _log.LogInformation("Actualizando la pelicula con ID: {Id}", id);
            return _peliculasService.UpdatePelicula(id, peliculaActualizada).ToActionResult();
        }
        
    }
}
*/
