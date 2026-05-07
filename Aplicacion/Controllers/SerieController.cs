
using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SerieController : ControllerBase
    {
        private readonly BdenviusaContext _context;
        private readonly ILogger<SerieController> _log;

        public SerieController(BdenviusaContext context, ILogger<SerieController> log)
        {
            _context = context;
            _log = log;

        }

       
        [HttpGet("VerSeries")]
        public IActionResult VerSeries()
        {
            var series = _context.Series.ToList();
            return Ok(series);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Serie serie)
        {
            try
            {   _log.LogInformation("Agregando una nueva serie: {Titulo}", serie.Titulo);
                _context.Series.Add(serie);
                _context.SaveChanges();
                _log.LogInformation("Serie agregada correctamente: {Titulo}", serie.Titulo);
                return Ok("Serie agregada correctamente");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error al agregar la serie");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Serie datos)
        {
            var serie = _context.Series.FirstOrDefault(s => s.IdSerie == id);

            if (serie == null)
                return NotFound("Serie no encontrada");

            serie.Titulo = datos.Titulo;
            serie.Director = datos.Director;
            serie.Duracion = datos.Duracion;
            serie.Poster = datos.Poster;
            serie.AnioEstreno = datos.AnioEstreno;
            serie.Plataforma = datos.Plataforma;
            serie.Estado = datos.Estado;

            _context.SaveChanges();
            return Ok("Serie actualizada correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var serie = _context.Series.FirstOrDefault(s => s.IdSerie == id);

            if (serie == null)
                return NotFound("Serie no encontrada");

            // Borrado lógico
            serie.Estado = 2;
            _context.SaveChanges();

            return Ok("Serie eliminada correctamente");
        }
    }
}
