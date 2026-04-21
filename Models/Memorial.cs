using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Memorial
{
    public int IdMemorial { get; set; }

    public int IdEmpleado { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual UsuarioTrabajador IdEmpleadoNavigation { get; set; } = null!;
}
