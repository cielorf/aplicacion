using Backend.Data.Models.MSSQL;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DomicilioController : ControllerBase
    {
        private readonly BdenviusaContext _context;

        public DomicilioController(BdenviusaContext context)
        {
            _context = context;
        }

        [HttpGet("VerDomicilios")]
        public IActionResult VerDomicilios()
        {
            var domicilios = _context.Domicilios.ToList();
            return Ok(domicilios);
        }

        [HttpPost("Agregar")]
        public IActionResult Agregar([FromBody] Domicilio domicilio)
        {
            try
            {
                _context.Domicilios.Add(domicilio);
                _context.SaveChanges();
                return Ok("Domicilio agregado correctamente");
            }
            catch (Exception ex)
            {
                // Si intentas agregar un segundo domicilio al mismo ID de usuario, 
               //Saldrá error debido a que es unico
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Error de SQL: {errorReal}");
            }
        }

        [HttpPut("Editar/{id}")]
        public IActionResult Editar(short id, [FromBody] Domicilio datos)
        {
            var dom = _context.Domicilios.FirstOrDefault(d => d.IdDomicilio == id);

            if (dom == null)
                return NotFound("Domicilio no encontrado");

            dom.Calle = datos.Calle;
            dom.NumeroExterior = datos.NumeroExterior;
            dom.NumeroInterior = datos.NumeroInterior;
            dom.Colonia = datos.Colonia;
            dom.Ciudad = datos.Ciudad;
            dom.Referencias = datos.Referencias;
            dom.Estado = datos.Estado;
           

            _context.SaveChanges();
            return Ok("Domicilio actualizado correctamente");
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(short id)
        {
            var dom = _context.Domicilios.FirstOrDefault(d => d.IdDomicilio == id);

            if (dom == null)
                return NotFound("Domicilio no encontrado");

            // Borrado lógico
            dom.Estado = 2;
            _context.SaveChanges();

            return Ok("Domicilio eliminado");
        }
    }
}