using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class HistorialEstadoMaquina
{
    public int IdHistorial { get; set; }

    public int IdMaquinaria { get; set; }

    public int IdEstadoCalidad { get; set; }

    public DateTime FechaCambio { get; set; }

    public string? Descripcion { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual EstadoCalidad IdEstadoCalidadNavigation { get; set; } = null!;

    public virtual Maquinarium IdMaquinariaNavigation { get; set; } = null!;
}
