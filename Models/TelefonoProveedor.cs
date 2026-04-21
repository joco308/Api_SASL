using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class TelefonoProveedor
{
    public int IdTelefono { get; set; }

    public int Telefono { get; set; }

    public int IdDetalle { get; set; }

    public int IdProveedor { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual SubDominio IdDetalleNavigation { get; set; } = null!;

    public virtual Provedore IdProveedorNavigation { get; set; } = null!;
}
