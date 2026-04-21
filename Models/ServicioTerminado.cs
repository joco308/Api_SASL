using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class ServicioTerminado
{
    public int IdServicioTerminado { get; set; }

    public int IdServicio { get; set; }

    public int Satisfaccion { get; set; }

    public string? Comentarios { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual SubDominio SatisfaccionNavigation { get; set; } = null!;
}
