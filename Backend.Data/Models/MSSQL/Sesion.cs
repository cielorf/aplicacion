using System;
using System.Collections.Generic;

namespace Backend.Data.Models.MSSQL;

public partial class Sesion
{
    public short IdSesion { get; set; }

    public DateTime FechaHora { get; set; }

    public byte Estado { get; set; }

    public short IdUsuario { get; set; }

  
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}