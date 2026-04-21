using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public int IdEmpresa { get; set; }

    public int IdDireccion { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string? ContactoEmergencia { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    public virtual SubDominio IdEmpresaNavigation { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<TelefonoCliente> TelefonoClientes { get; set; } = new List<TelefonoCliente>();
}
