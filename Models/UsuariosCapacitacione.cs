using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class UsuariosCapacitacione
{
    public int IdUsuario { get; set; }

    public int IdCapacitacion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Capacitacione IdCapacitacionNavigation { get; set; } = null!;

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
