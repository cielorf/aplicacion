namespace Aplicacion.Models
{
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