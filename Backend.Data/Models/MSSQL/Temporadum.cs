using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Temporadum
{
    public short IdTemporada { get; set; }

    public byte NumeroTemporada { get; set; }

    public short Episodios { get; set; }

    public string Sinopsis { get; set; } = null!;

    public byte Estado { get; set; }

    public virtual ICollection<Serie> IdSeries { get; set; } = new List<Serie>();
}
