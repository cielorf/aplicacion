using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ILogger<PeliculasController> _log;

        public PeliculasController(ILogger<PeliculasController> log)
        {
            _log = log;
        }

        private List<Pelicula> pelicula = new List<Pelicula>
        {
            new Pelicula
            {
                Id = 1,
                Titulo = "Spider-Man",
                Director = "Sam Raimi",
                Genero = "Accion",
                DuracionMinutos = 121,
                PrecioRecaudacion = 8,
                FechaEstreno = DateOnly.Parse("2002-05-17"),
                Activa = true
            },
            new Pelicula
            {
                Id = 2,
                Titulo = "Avengers: Endgame",
                Director = "Anthony y Joe Russo",
                Genero = "Accion",
                DuracionMinutos = 121,
                 PrecioRecaudacion = 8,
                FechaEstreno = DateOnly.Parse("2019-04-26"),
                Activa = true
            },
            new Pelicula
            {
                Id = 3,
                Titulo = "El curioso caso de Benjamin Button",
                Director = "David Fincher",
                Genero = "Drama",
                DuracionMinutos = 121,
                 PrecioRecaudacion = 8,
                FechaEstreno = DateOnly.Parse("2009-01-16"),
                Activa = true
            },
            new Pelicula
            {
                Id = 4,
                Titulo = "Perfume: la historia de un asesino",
                Director = "Tom Tykwer",
                Genero = "Ficción",
                DuracionMinutos = 121,
                 PrecioRecaudacion = 8,
                FechaEstreno = DateOnly.Parse("2006-09-14"),
                Activa = true
            },
            new Pelicula
            {
                Id = 5,
                Titulo = "Un amor para recordar",
                Director = "Adam Shankman",
                Genero = "Romance",
                DuracionMinutos = 121,
                PrecioRecaudacion = 8,
                FechaEstreno = DateOnly.Parse("2002-06-21"),
                Activa = true
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
                .Where(c => c.Activa == Activo)
                .Select(c => new
                {
                    c.Titulo,
                    c.Director,
                    c.FechaEstreno
                })
                .ToList();

            return Ok(lst);
        }

        //Agregar
        [HttpPost("Agregar")]
        public IActionResult Add(Pelicula nuevapelicula)
        {
            List<string> ListaErrores = new List<string>();
            if (string.IsNullOrEmpty(nuevapelicula.Titulo))
                ListaErrores.Add("El Titulo no puede estar vacio");
            if (string.IsNullOrEmpty(nuevapelicula.Director))
                ListaErrores.Add("El Director no puede estar vacio");
            if (string.IsNullOrEmpty(nuevapelicula.Genero))
                ListaErrores.Add("El Genero no puede estar vacio");
            if (nuevapelicula.DuracionMinutos == 0)
                ListaErrores.Add("La duración en minutos no puede estar vacio");
            if (nuevapelicula.PrecioRecaudacion == 0)
                ListaErrores.Add("El precio de recaudación no puede estar vacio");

            if (!ListaErrores.Any())
            {
                pelicula.Add(nuevapelicula);
                return Ok("Película agregada con éxito");

            }

            _log.LogError("A ocurrido un error al agregar una pelicula " + string.Join(",", ListaErrores));

            return BadRequest(ListaErrores);
        }

        [HttpDelete("Borrar")]
        public IActionResult Delete(int id)
        {
            // Validar que el id no sea 0 o negativo
            if (id <= 0)
            {
                return BadRequest("El id no puede ser 0 o negativo");
            }

            var PeliculaAborrar = pelicula.Where(c => c.Id == id).FirstOrDefault();

            // Validar que exista
            if (PeliculaAborrar == null)
            {
                return BadRequest("La pelicula no existe");
            }

            // Validar que no esté ya desactivada
            if (!PeliculaAborrar.Activa)
            {
                return BadRequest("La pelicula ya está desactivada");
            }

            pelicula.Remove(PeliculaAborrar);

            PeliculaAborrar.Activa = false;

            pelicula.Add(PeliculaAborrar);

            return Ok(pelicula.Where(p => p.Id == id).FirstOrDefault());
        }

        // Actualizar
        [HttpPut("Actualizar/{id}")]
        public IActionResult Update(int id, Pelicula peliculaActualizada)
        {
            List<string> ListaErrores = new List<string>();


            var peliculaExistente = pelicula.FirstOrDefault(c => c.Id == id);

            if (peliculaExistente == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(peliculaActualizada.Titulo))
                ListaErrores.Add("El Titulo no puede estar vacio");
            if (string.IsNullOrEmpty(peliculaActualizada.Director))
                ListaErrores.Add("El Director no puede estar vacio");
            if (string.IsNullOrEmpty(peliculaActualizada.Genero))
                ListaErrores.Add("El Genero no puede estar vacio");
            if (peliculaActualizada.DuracionMinutos == 0)
                ListaErrores.Add("La duración en minutos no puede estar vacio");
            if (peliculaActualizada.PrecioRecaudacion == 0)
                ListaErrores.Add("El precio de recaudación no puede estar vacio");

            if (ListaErrores.Any())
            {
                _log.LogError("A ocurrido un error al agregar una pelicula " + string.Join(",", ListaErrores));
                return BadRequest(ListaErrores);
            }

            // Actualizamos campos
            peliculaExistente = peliculaActualizada;
            peliculaExistente.Id = id;
           

            return Ok(peliculaExistente);
        }
    }

    public class Pelicula
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Director { get; set; }
        public string Genero { get; set; }
        public int DuracionMinutos { get; set; }
        public string DuracionString { get => DuracionMinutos.ToString() + " minutos."; }
        public decimal PrecioRecaudacion { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public bool Activa { get; set; }

        // Propiedad calculada
        public int Antiguedad => DateTime.Now.Year - FechaEstreno.Year;
    }
}