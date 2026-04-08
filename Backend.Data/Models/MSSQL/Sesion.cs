using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Sesion
{
    public short IdSesion { get; set; }

    public string Nickname { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public short IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
