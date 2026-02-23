using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private List<Serie> series = new List<Serie>
        {
            new Serie
            {
                Id = 1,
                Titulo = "Breaking Bad",
                Genero = "Drama",
                Temporadas = 5,
                AnioEstreno = 2008,
                Activa = false
            },
            new Serie
            {
                Id = 2,
                Titulo = "Stranger Things",
                Genero = "Ciencia ficción",
                Temporadas = 4,
                AnioEstreno = 2016,
                Activa = true
            }
        };

        // GET TODOS
        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            return Ok(series);
        }

        // GET ACTIVOS
        [HttpGet("VerActivas")]
        public IActionResult GetActivas()
        {
            var lista = series.Where(s => s.Activa == true).ToList();
            return Ok(lista);
        }

        // POST
        [HttpPost("Agregar")]
        public IActionResult Add(Serie serie)
        {
            series.Add(serie);
            return Ok(series);
        }

        // DELETE
        [HttpDelete("Borrar")]
        public IActionResult Delete(int id)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);

            if (serie == null)
                return NotFound();

            series.Remove(serie);
            serie.Activa = false;
            series.Add(serie);

            return Ok(series);
        }

        // PUT
        [HttpPut("Actualizar")]
        public IActionResult Update(int id, Serie serieActualizada)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);

            if (serie == null)
                return NotFound();

            serie.Titulo = serieActualizada.Titulo;
            serie.Genero = serieActualizada.Genero;
            serie.Temporadas = serieActualizada.Temporadas;
            serie.AnioEstreno = serieActualizada.AnioEstreno;
            serie.Activa = serieActualizada.Activa;

            return Ok(serie);
        }
    }

    public class Serie
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public int Temporadas { get; set; }
        public int AnioEstreno { get; set; }
        public bool Activa { get; set; }
    }
}