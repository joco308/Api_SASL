using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Maquinarium
{
    public int IdMaquinaria { get; set; }

    public int IdProveedor { get; set; }

    public int IdTipoMaquinaria { get; set; }

    public int IdEstadoCalidad { get; set; }

    public int IdMarcaMaquinaria { get; set; }

    public string NombreMaquinaria { get; set; } = null!;

    public string CodigoInv { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionMaquinarium> AsignacionMaquinaria { get; set; } = new List<AsignacionMaquinarium>();

    public virtual ICollection<HistorialEstadoMaquina> HistorialEstadoMaquinas { get; set; } = new List<HistorialEstadoMaquina>();

    public virtual EstadoCalidad IdEstadoCalidadNavigation { get; set; } = null!;

    public virtual MarcaMaquinarium IdMarcaMaquinariaNavigation { get; set; } = null!;

    public virtual Provedore IdProveedorNavigation { get; set; } = null!;

    public virtual SubDominio IdTipoMaquinariaNavigation { get; set; } = null!;

    public virtual ICollection<MantenimientosMaquinarium> MantenimientosMaquinaria { get; set; } = new List<MantenimientosMaquinarium>();
}
