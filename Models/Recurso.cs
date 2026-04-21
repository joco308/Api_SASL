using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Recurso
{
    public int IdRecurso { get; set; }

    public int IdProveedor { get; set; }

    public int IdTipo { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionRecurso> AsignacionRecursos { get; set; } = new List<AsignacionRecurso>();

    public virtual Provedore IdProveedorNavigation { get; set; } = null!;

    public virtual SubDominio IdTipoNavigation { get; set; } = null!;
}
