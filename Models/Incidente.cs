using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Incidente
{
    public int IdIncidente { get; set; }

    public int IdServicio { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
