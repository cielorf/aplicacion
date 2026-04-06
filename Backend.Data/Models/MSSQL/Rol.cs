using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Rol
{
    public short IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
