using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeliculaController : ControllerBase
    {
        private readonly ILogger<SerieController> _log;
        private readonly BdenviusaContext _context;

        public PeliculaController(BdenviusaContext context, ILogger<SerieController> log)
        {
            _context = context;
            _log = log;
        }


        [HttpGet("VerPeliculas")]
        public IActionResult VerPeliculas()
        {
            var peliculas = _context.Peliculas
                .Where(p => p.Estado != 2)
                .ToList();

            return Ok(peliculas);
        }
        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Pelicula pelicula)
        {
           try
            {
                _log.LogInformation("Agregando una nueva película: {Titulo}", pelicula.Titulo);
                _context.Peliculas.Add(pelicula);
                _log.LogInformation("Guardando cambios en la base de datos para la película: {Titulo}", pelicula.Titulo);
                _context.SaveChanges();
                _log.LogInformation("Película agregada correctamente: {Titulo}", pelicula.Titulo);
                return Ok("Película agregada correctamente");

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar la película");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Pelicula datos)
        {
            try 
            {
                _log.LogInformation("Editando la película con ID: {Id}", id);
                var pelicula = _context.Peliculas.FirstOrDefault(p => p.IdPelicula == id);
                if (pelicula == null)
                    return NotFound("Película no encontrada");
                pelicula.Titulo = datos.Titulo;
                pelicula.Director = datos.Director;
                pelicula.Duracion = datos.Duracion;
                pelicula.Poster = datos.Poster;
                pelicula.AnioEstreno = datos.AnioEstreno;
                pelicula.PrecioRecaudacion = datos.PrecioRecaudacion;
                pelicula.Estado = datos.Estado;
                _context.SaveChanges();
                _log.LogInformation("Película editada correctamente: {Titulo}", pelicula.Titulo);
                return Ok("Película editada correctamente");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al editar la película");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var pelicula = _context.Peliculas.FirstOrDefault(p => p.IdPelicula == id);

            if (pelicula == null)
                return NotFound("Película no encontrada");

            // Borrado lógico
            pelicula.Estado = 2;

            _context.SaveChanges();

            return Ok("Película eliminada correctamente");
        }
    }
}