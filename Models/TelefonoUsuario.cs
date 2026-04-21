using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class TelefonoUsuario
{
    public int IdTelefonoUsuario { get; set; }

    public int TelefonoUsuario1 { get; set; }

    public int IdUsuario { get; set; }

    public int IdDetalle { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual SubDominio IdDetalleNavigation { get; set; } = null!;

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
