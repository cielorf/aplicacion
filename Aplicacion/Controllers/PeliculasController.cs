using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private List<Pelicula> pelicula = new List<Pelicula>
        {
            new Pelicula
            {
                id = 1,
                titulo = "Spider-Man",
                director = "Sam Raimi",
                genero = "Accion",
                Duracion = "121 minutos",
                FechaEstreno = DateOnly.Parse("2002-05-17"),
                activo = true
            },
            new Pelicula
            {
                id = 2,
                titulo = "Avengers: Endgame",
                director = "Anthony y Joe Russo",
                genero = "Accion",
                Duracion = "182 minutos",
                FechaEstreno = DateOnly.Parse("2019-04-26"),
                activo = true
            },
            new Pelicula
            {
                id = 3,
                titulo = "El curioso caso de Benjamin Button",
                director = "David Fincher",
                genero = "Drama",
                Duracion = "168 minutos",
                FechaEstreno = DateOnly.Parse("2009-01-16"),
                activo = true
            },
            new Pelicula
            {
                id = 4,
                titulo = "Perfume: la historia de un asesino",
                director = "Tom Tykwer",
                genero = "Ficción",
                Duracion = "147 minutos",
                FechaEstreno = DateOnly.Parse("2006-09-14"),
                activo = true
            },
            new Pelicula
            {
                id = 5,
                titulo = "Un amor para recordar",
                director = "Adam Shankman",
                genero = "Romance",
                Duracion = "101 minutos",
                FechaEstreno = DateOnly.Parse("2002-06-21"),
                activo = true
            }
        };

        //Ver
        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            return Ok(pelicula);
        }

        [HttpGet("VerActivos")]
        public IActionResult Get(bool Activo)
        {
            var lst = pelicula
                .Where(c => c.activo == Activo)
                .Select(c => new
                {
                    c.titulo,
                    c.director,
                    c.FechaEstreno
                })
                .ToList();

            return Ok(lst);
        }

        //Agregar
        [HttpPost("Agregar")]
        public IActionResult Add(Pelicula nuevapelicula)
        {
            pelicula.Add(nuevapelicula);

            return Ok(pelicula);
        }

        [HttpDelete("Borrar")]
        public IActionResult Delete(int id)
        {
            var PeliculaAborrar = pelicula.Where(c => c.id == id).FirstOrDefault();

            if (PeliculaAborrar == null)
            {
                return NotFound();
            }

            pelicula.Remove(PeliculaAborrar);

            PeliculaAborrar.activo = false;

            pelicula.Add(PeliculaAborrar);

            return Ok(pelicula);
        }

        // Actualizar
        [HttpPut("Actualizar")]
        public IActionResult Update(int id, Pelicula peliculaActualizada)
        {
            var peliculaExistente = pelicula.FirstOrDefault(c => c.id == id);

            if (peliculaExistente == null)
            {
                return NotFound();
            }

            // Actualizamos campos
            peliculaExistente.titulo = peliculaActualizada.titulo;
            peliculaExistente.director = peliculaActualizada.director;
            peliculaExistente.genero = peliculaActualizada.genero;
            peliculaExistente.Duracion = peliculaActualizada.Duracion;
            peliculaExistente.FechaEstreno = peliculaActualizada.FechaEstreno;
            peliculaExistente.activo = peliculaActualizada.activo;

            return Ok(peliculaExistente);
        }
    }

    public class Pelicula
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string director { get; set; }
        public string genero { get; set; }
        public string Duracion { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public bool activo { get; set; }
    }
}