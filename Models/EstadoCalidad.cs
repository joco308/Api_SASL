using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class EstadoCalidad
{
    public int IdEstadoCalidad { get; set; }

    public string EstadoCalidad1 { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<HistorialEstadoMaquina> HistorialEstadoMaquinas { get; set; } = new List<HistorialEstadoMaquina>();

    public virtual ICollection<Maquinarium> Maquinaria { get; set; } = new List<Maquinarium>();
}
