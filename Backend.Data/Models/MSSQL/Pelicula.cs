using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Pelicula
{
    public short IdPelicula { get; set; }

    public string Titulo { get; set; } = null!;

    public string Director { get; set; } = null!;

    public short Duracion { get; set; }

    public string Resumen { get; set; } = null!;

    public string Poster { get; set; } = null!;

    public int PrecioRecaudacion { get; set; }

    public short AnioEstreno { get; set; }

    public byte Estado { get; set; }

    public virtual ICollection<Actor> IdActors { get; set; } = new List<Actor>();

    public virtual ICollection<Genero> IdGeneros { get; set; } = new List<Genero>();
}
