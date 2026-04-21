using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class AsignacionMaquinarium
{
    public int IdServicio { get; set; }

    public int IdMaquinaria { get; set; }

    public int Cantidad { get; set; }

    public string? Descripcion { get; set; }

    public virtual Maquinarium IdMaquinariaNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
