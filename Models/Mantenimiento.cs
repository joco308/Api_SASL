using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Mantenimiento
{
    public int IdMantenimiento { get; set; }

    public DateOnly FechaMantenimiento { get; set; }

    public string? Descripcion { get; set; }

    public decimal Costo { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<MantenimientosMaquinarium> MantenimientosMaquinaria { get; set; } = new List<MantenimientosMaquinarium>();
}
