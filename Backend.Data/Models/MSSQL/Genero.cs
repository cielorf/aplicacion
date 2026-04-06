using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Genero
{
    public short IdGenero { get; set; }

    public string Nombre { get; set; } = null!;

    public byte Estado { get; set; }

    public virtual ICollection<Pelicula> IdPeliculas { get; set; } = new List<Pelicula>();

    public virtual ICollection<Serie> IdSeries { get; set; } = new List<Serie>();
}
