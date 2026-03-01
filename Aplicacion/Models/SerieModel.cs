namespace Aplicacion.Models
{
    // Modelo Serie
    public class Serie
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Plataforma { get; set; }
        public int AnioEstreno { get; set; }
        public int Antiguedad => DateTime.Now.Year - AnioEstreno;
        public required string Genero { get; set; }
        public bool Activa { get; set; }
        public List<TemporadaInfo> TemporadasEpisodios { get; set; } = new();
        public int Temporadas => TemporadasEpisodios.Sum(t => t.Temporadas);
        public int Episodios => TemporadasEpisodios.Sum(t => t.Episodios);
        public double PromedioEpisodiosPorTemporada =>
            Temporadas == 0 ? 0 : (double)Episodios / Temporadas;
    }

    // Clase auxiliar
    public class TemporadaInfo
    {
        public int Temporadas { get; set; }
        public int Episodios { get; set; }
    }
}
