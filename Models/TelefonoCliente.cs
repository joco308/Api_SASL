using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class TelefonoCliente
{
    public int IdTelefono { get; set; }

    public int Telefono { get; set; }

    public int IdDetalle { get; set; }

    public int IdCliente { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual SubDominio IdDetalleNavigation { get; set; } = null!;
}
