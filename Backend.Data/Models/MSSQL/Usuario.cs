using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Usuario
{
    public short IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public byte Estado { get; set; }

    public short IdRol { get; set; }

    public virtual Domicilio? Domicilio { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Sesion> Sesions { get; set; } = new List<Sesion>();
}
