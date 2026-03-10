using Backend.Data.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _log;
        private readonly SeriesService _seriesService;

        public SeriesController(SeriesService seriesService, ILogger<SeriesController> log)
        {
            _seriesService = seriesService;
            _log = log;
        }

        // Ver todos
        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            _log.LogInformation("Obteniendo todas las series");
            return _seriesService.GetSeries().ToActionResult();
        }

        // Agregar
        [HttpPost("Agregar")]
        public IActionResult Add(Serie nuevaSerie)
        {
            _log.LogInformation("Agregando una nueva serie");
            return _seriesService.AddSerie(nuevaSerie).ToActionResult();
        }

        // Borrar
        [HttpDelete("Borrar/{id}")]
        public IActionResult Delete(string id)
        {
            _log.LogInformation("Eliminando la serie con ID: {Id}", id);
            return _seriesService.DeleteSerie(id).ToActionResult();
        }

        // Actualizar
        [HttpPut("Actualizar/{id}")]
        public IActionResult Update(string id, Serie serieActualizada)
        {
            _log.LogInformation("Actualizando la serie con ID: {Id}", id);
            return _seriesService.UpdateSerie(id, serieActualizada).ToActionResult();
        }
    }
}