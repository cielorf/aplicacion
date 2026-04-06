using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeliculaController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public PeliculaController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerPeliculas")]
        public IActionResult VerPeliculas()
        {
            var peliculas = _context.Peliculas.ToList();
            return Ok(peliculas);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Pelicula pelicula)
        {
            _context.Peliculas.Add(pelicula);
            _context.SaveChanges();
            return Ok("Película agregada correctamente");
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Pelicula datos)
        {
            var pelicula = _context.Peliculas.FirstOrDefault(p => p.IdPelicula == id);

            if (pelicula == null)
                return NotFound("Película no encontrada");

            pelicula.Titulo = datos.Titulo;
            pelicula.Director = datos.Director;
            pelicula.Duracion = datos.Duracion;
            pelicula.Resumen = datos.Resumen;
            pelicula.Poster = datos.Poster;
            pelicula.PrecioRecaudacion = datos.PrecioRecaudacion;
            pelicula.AnioEstreno = datos.AnioEstreno;
            pelicula.Estado = datos.Estado;

            _context.SaveChanges();
            return Ok("Película actualizada correctamente");
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