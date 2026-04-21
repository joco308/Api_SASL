using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class AsignacionRecurso
{
    public int IdServicio { get; set; }

    public int IdRecurso { get; set; }

    public int Cantidad { get; set; }

    public virtual Recurso IdRecursoNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
