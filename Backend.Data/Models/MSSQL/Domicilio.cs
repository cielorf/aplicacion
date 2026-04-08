using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Domicilio
{
    public short IdDomicilio { get; set; }

    public string Calle { get; set; } = null!;

    public string NumeroExterior { get; set; } = null!;

    public string NumeroInterior { get; set; } = null!;

    public string Colonia { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Referencias { get; set; } = null!;

    public byte Estado { get; set; }

    public short IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
