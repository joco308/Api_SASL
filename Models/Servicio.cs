using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public int IdCliente { get; set; }

    public int IdDireccion { get; set; }

    public int TipoServicio { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFinal { get; set; }

    public decimal Costo { get; set; }

    public string? Descripcion { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionEmpleado> AsignacionEmpleados { get; set; } = new List<AsignacionEmpleado>();

    public virtual ICollection<AsignacionMaquinarium> AsignacionMaquinaria { get; set; } = new List<AsignacionMaquinarium>();

    public virtual ICollection<AsignacionRecurso> AsignacionRecursos { get; set; } = new List<AsignacionRecurso>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    public virtual ICollection<Incidente> Incidentes { get; set; } = new List<Incidente>();

    public virtual ICollection<ServicioTerminado> ServicioTerminados { get; set; } = new List<ServicioTerminado>();

    public virtual SubDominio TipoServicioNavigation { get; set; } = null!;
}
