using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Actor
{
    public short IdActor { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public DateOnly FechaNac { get; set; }

    public string Poster { get; set; } = null!;

    public byte Estado { get; set; }

    public virtual ICollection<Pelicula> IdPeliculas { get; set; } = new List<Pelicula>();

    public virtual ICollection<Serie> IdSeries { get; set; } = new List<Serie>();
}
