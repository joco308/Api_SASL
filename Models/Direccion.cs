using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Direccion
{
    public int IdDireccion { get; set; }

    public int IdZona { get; set; }

    public string Calle { get; set; } = null!;

    public int NCasa { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual SubDominio IdZonaNavigation { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<UsuarioTrabajador> UsuarioTrabajadors { get; set; } = new List<UsuarioTrabajador>();
}
