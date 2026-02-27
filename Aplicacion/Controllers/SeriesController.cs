using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _log;

        public SeriesController(ILogger<SeriesController> log)
        {
            _log = log;
        }

        private List<Serie> serie = new List<Serie>
        {
            new Serie
            {
                Id = 1,
                Titulo = "Breaking Bad",
                Plataforma = "Netflix",
                AnioEstreno = 2008,
                Genero = "Drama",
                Activa = true,
                TemporadasEpisodios = new List<(int, int)>
                {
                    (1, 7),
                    (1, 13),
                    (1, 13),
                    (1, 13),
                    (1, 16)
                }
            },
            new Serie
            {
                Id = 2,
                Titulo = "Stranger Things",
                Plataforma = "Netflix",
                AnioEstreno = 2016,
                Genero = "Ciencia Ficción",
                Activa = true,
                TemporadasEpisodios = new List<(int, int)>
                {
                    (1, 8),
                    (1, 9),
                    (1, 8),
                    (1, 9)
                }
            }
        };

        // Ver todos
        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            return Ok(serie);
        }

        // Ver activos
        [HttpGet("VerActivos")]
        public IActionResult Get(bool Activo)
        {
            var lst = serie
                .Where(s => s.Activa == Activo)
                .Select(s => new
                {
                    s.Titulo,
                    s.Plataforma,
                    s.AnioEstreno
                })
                .ToList();

            return Ok(lst);
        }

        // Agregar
        [HttpPost("Agregar")]
        public IActionResult Add(Serie nuevaSerie)
        {
            List<string> ListaErrores = new List<string>();

            if (string.IsNullOrEmpty(nuevaSerie.Titulo))
                ListaErrores.Add("El Titulo no puede estar vacio");

            if (string.IsNullOrEmpty(nuevaSerie.Plataforma))
                ListaErrores.Add("La Plataforma no puede estar vacia");

            if (nuevaSerie.AnioEstreno == 0)
                ListaErrores.Add("El Año de estreno no puede estar vacio");

            if (string.IsNullOrEmpty(nuevaSerie.Genero))
                ListaErrores.Add("El Genero no puede estar vacio");

            if (nuevaSerie.TemporadasEpisodios == null || !nuevaSerie.TemporadasEpisodios.Any())
                ListaErrores.Add("Debe tener al menos una temporada");

            if (!ListaErrores.Any())
            {
                serie.Add(nuevaSerie);
                return Ok(serie);
            }

            _log.LogError("Error al agregar serie: " + string.Join(",", ListaErrores));
            return BadRequest(ListaErrores);
        }

        // Borrar (desactivar)
        [HttpDelete("Borrar")]
        public IActionResult Delete(int id)
        {
            var serieABorrar = serie.FirstOrDefault(s => s.Id == id);

            if (serieABorrar == null)
                return NotFound();

            serieABorrar.Activa = false;

            return Ok(serie);
        }

        // Actualizar
        [HttpPut("Actualizar")]
        public IActionResult Update(int id, Serie serieActualizada)
        {
            List<string> ListaErrores = new List<string>();

            var serieExistente = serie.FirstOrDefault(s => s.Id == id);

            if (serieExistente == null)
                return NotFound();

            if (string.IsNullOrEmpty(serieActualizada.Titulo))
                ListaErrores.Add("El Titulo no puede estar vacio");

            if (string.IsNullOrEmpty(serieActualizada.Plataforma))
                ListaErrores.Add("La Plataforma no puede estar vacia");

            if (serieActualizada.AnioEstreno == 0)
                ListaErrores.Add("El Año de estreno no puede estar vacio");

            if (string.IsNullOrEmpty(serieActualizada.Genero))
                ListaErrores.Add("El Genero no puede estar vacio");

            if (ListaErrores.Any())
            {
                _log.LogError("Error al actualizar serie: " + string.Join(",", ListaErrores));
                return BadRequest(ListaErrores);
            }

            // Actualizar campos correctamente
            serieExistente.Titulo = serieActualizada.Titulo;
            serieExistente.Plataforma = serieActualizada.Plataforma;
            serieExistente.AnioEstreno = serieActualizada.AnioEstreno;
            serieExistente.Genero = serieActualizada.Genero;
            serieExistente.TemporadasEpisodios = serieActualizada.TemporadasEpisodios;
            serieExistente.Activa = serieActualizada.Activa;

            return Ok(serieExistente);
        }
    }

    public class Serie
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string Plataforma { get; set; }
        public int AnioEstreno { get; set; }

        public int Antiguedad => DateTime.Now.Year - AnioEstreno;

        public string Genero { get; set; }
        public bool Activa { get; set; }

        public List<(int Temporadas, int Episodios)> TemporadasEpisodios { get; set; }

        public int Temporadas => TemporadasEpisodios?.Sum(te => te.Temporadas) ?? 0;

        public int Episodios => TemporadasEpisodios?.Sum(te => te.Episodios) ?? 0;

        public double PromedioEpisodiosPorTemporada =>
            Temporadas == 0 ? 0 : (double)Episodios / Temporadas;
    }
}