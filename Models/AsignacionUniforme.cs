using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class AsignacionUniforme
{
    public int IdAsignacionUniforme { get; set; }

    public int IdUsuario { get; set; }

    public int IdUniforme { get; set; }

    public DateOnly FechaEntrega { get; set; }

    public DateOnly? FechaDevolucion { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Uniforme IdUniformeNavigation { get; set; } = null!;

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
