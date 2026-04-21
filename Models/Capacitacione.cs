using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Capacitacione
{
    public int IdCapacitacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateOnly Fecha { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<UsuariosCapacitacione> UsuariosCapacitaciones { get; set; } = new List<UsuariosCapacitacione>();
}
