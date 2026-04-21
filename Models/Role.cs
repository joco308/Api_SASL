using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public int Salario { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<UsuarioTrabajador> UsuarioTrabajadors { get; set; } = new List<UsuarioTrabajador>();
}
