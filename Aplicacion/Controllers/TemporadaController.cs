using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemporadaController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public TemporadaController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerTemporadas")]
        public IActionResult VerTemporadas()
        {
          
            var temporadas = _context.Temporada.Where(t => t.Estado == 1).ToList();
            return Ok(temporadas);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Temporadum temporada)
        {
            try
            {
                _context.Temporada.Add(temporada);
                _context.SaveChanges();
                return Ok("Temporada agregada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar temporada: {ex.Message}");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Temporadum datos)
        {
            // Buscamos por IdTemporada (PK id_temporada)
            var temporada = _context.Temporada.FirstOrDefault(t => t.IdTemporada == id);

            if (temporada == null)
                return NotFound("Temporada no encontrada");

            // Mapeo
            temporada.NumeroTemporada = datos.NumeroTemporada;
            temporada.Episodios = datos.Episodios;
            temporada.Sinopsis = datos.Sinopsis;
            temporada.Estado = datos.Estado;

            _context.SaveChanges();
            return Ok("Temporada actualizada correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var temporada = _context.Temporada.FirstOrDefault(t => t.IdTemporada == id);

            if (temporada == null)
                return NotFound("Temporada no encontrada");

            // Borrado lógico (Estado = 2)
            temporada.Estado = 2;

            _context.SaveChanges();
            return Ok("Temporada eliminada correctamente");
        }
    }
}