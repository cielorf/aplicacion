
using Backend.Data.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ILogger<PeliculasController> _log;
        private readonly PeliculasService _peliculasService;

        public PeliculasController(PeliculasService peliculasService, ILogger<PeliculasController> log)
        {
            _peliculasService = peliculasService;
            _log = log;
        }

        // Ver todas
        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            _log.LogInformation("Obteniendo todas las peliculas");
            return _peliculasService.GetPeliculas().ToActionResult();
        }

        // Agregar
        [HttpPost("Agregar")]
        public IActionResult Add(Pelicula nuevaPelicula)
        {
            _log.LogInformation("Agregando una nueva pelicula");
            return _peliculasService.AddPelicula(nuevaPelicula).ToActionResult();
        }

        // Borrar
        [HttpDelete("Borrar/{id}")]
        public IActionResult Delete(string id)
        {
            _log.LogInformation("Eliminando la pelicula con ID: {Id}", id);
            return _peliculasService.DeletePelicula(id).ToActionResult();
        }

        // Actualizar
        [HttpPut("Actualizar/{id}")]
        public IActionResult Update(string id, Pelicula peliculaActualizada)
        {
            _log.LogInformation("Actualizando la pelicula con ID: {Id}", id);
            return _peliculasService.UpdatePelicula(id, peliculaActualizada).ToActionResult();
        }
    }
}