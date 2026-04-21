using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class MantenimientosMaquinarium
{
    public int IdMaquinaria { get; set; }

    public int IdMantenimiento { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Mantenimiento IdMantenimientoNavigation { get; set; } = null!;

    public virtual Maquinarium IdMaquinariaNavigation { get; set; } = null!;
}
