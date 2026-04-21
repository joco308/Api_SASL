using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Provedore
{
    public int IdProveedor { get; set; }

    public int IdEmpresa { get; set; }

    public int IdProducto { get; set; }

    public int Nit { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual SubDominio IdEmpresaNavigation { get; set; } = null!;

    public virtual SubDominio IdProductoNavigation { get; set; } = null!;

    public virtual ICollection<Maquinarium> Maquinaria { get; set; } = new List<Maquinarium>();

    public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();

    public virtual ICollection<TelefonoProveedor> TelefonoProveedors { get; set; } = new List<TelefonoProveedor>();
}
