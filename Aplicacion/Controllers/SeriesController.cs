using Microsoft.AspNetCore.Mvc;
using Aplicacion.Models;
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
                TemporadasEpisodios = new List<TemporadaInfo>
                {
                    new TemporadaInfo { Temporadas = 1, Episodios = 7 },
                    new TemporadaInfo { Temporadas = 1, Episodios = 13 },
                    new TemporadaInfo { Temporadas = 1, Episodios = 13 }
                }
            }
        };

        // Ver todos
        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            return Ok(serie);
        }

        // Agregar
        [HttpPost("Agregar")]
        public IActionResult Add(Serie nuevaSerie)
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrEmpty(nuevaSerie.Titulo))
                errores.Add("El Titulo no puede estar vacio");

            if (string.IsNullOrEmpty(nuevaSerie.Plataforma))
                errores.Add("La Plataforma no puede estar vacia");

            if (nuevaSerie.AnioEstreno == 0)
                errores.Add("El Año de estreno no puede estar vacio");

            if (string.IsNullOrEmpty(nuevaSerie.Genero))
                errores.Add("El Genero no puede estar vacio");

            if (nuevaSerie.TemporadasEpisodios == null ||
                !nuevaSerie.TemporadasEpisodios.Any())
                errores.Add("Debe tener al menos una temporada");

            if (nuevaSerie.TemporadasEpisodios != null)
            {
                foreach (var temp in nuevaSerie.TemporadasEpisodios)
                {
                    if (temp.Temporadas <= 0)
                        errores.Add("La temporada no puede ser 0");

                    if (temp.Episodios <= 0)
                        errores.Add("El episodio no puede ser 0");
                }
            }

            if (errores.Any())
                return BadRequest(errores);

            serie.Add(nuevaSerie);

            return Ok("Serie agregada con éxito");
        }

        // Borrar
        [HttpDelete("Borrar/{id}")]
        public IActionResult Delete(int id)
        {
            var serieABorrar = serie.FirstOrDefault(s => s.Id == id);

            if (serieABorrar == null)
                return BadRequest("La serie no existe");

            serieABorrar.Activa = false;

            return Ok("Serie eliminada con éxito");
        }

        // Actualizar
        [HttpPut("Actualizar/{id}")]
        public IActionResult Update(int id, Serie serieActualizada)
        {
            List<string> errores = new List<string>();

            var serieExistente = serie.FirstOrDefault(s => s.Id == id);

            if (serieExistente == null)
                return NotFound();

            if (string.IsNullOrEmpty(serieActualizada.Titulo))
                errores.Add("El Titulo no puede estar vacio");

            if (string.IsNullOrEmpty(serieActualizada.Plataforma))
                errores.Add("La Plataforma no puede estar vacia");

            if (serieActualizada.AnioEstreno == 0)
                errores.Add("El Año de estreno no puede estar vacio");

            if (string.IsNullOrEmpty(serieActualizada.Genero))
                errores.Add("El Genero no puede estar vacio");

            if (serieActualizada.TemporadasEpisodios == null ||
                !serieActualizada.TemporadasEpisodios.Any())
                errores.Add("Debe tener al menos una temporada");

            if (serieActualizada.TemporadasEpisodios != null)
            {
                foreach (var temp in serieActualizada.TemporadasEpisodios)
                {
                    if (temp.Temporadas <= 0)
                        errores.Add("La temporada no puede ser 0");

                    if (temp.Episodios <= 0)
                        errores.Add("El episodio no puede ser 0");
                }
            }

            if (errores.Any())
                return BadRequest(errores);

            // Actualizar campos correctamente
            serieExistente.Titulo = serieActualizada.Titulo;
            serieExistente.Plataforma = serieActualizada.Plataforma;
            serieExistente.AnioEstreno = serieActualizada.AnioEstreno;
            serieExistente.Genero = serieActualizada.Genero;
            serieExistente.TemporadasEpisodios = serieActualizada.TemporadasEpisodios;
            serieExistente.Activa = serieActualizada.Activa;

            return Ok("Serie actualizada con éxito");
        }
    }

    
}