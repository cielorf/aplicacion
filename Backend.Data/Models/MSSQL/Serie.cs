using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Serie
{
    public short IdSerie { get; set; }

    public string Titulo { get; set; } = null!;

    public string Director { get; set; } = null!;

    public short Duracion { get; set; }

    public string Poster { get; set; } = null!;

    public short AnioEstreno { get; set; }

    public string Plataforma { get; set; } = null!;

    public byte Estado { get; set; }

    public virtual ICollection<Actor> IdActors { get; set; } = new List<Actor>();

    public virtual ICollection<Genero> IdGeneros { get; set; } = new List<Genero>();

    public virtual ICollection<Temporadum> IdTemporada { get; set; } = new List<Temporadum>();
}
