using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Qr
{
    public int IdQr { get; set; }

    public int IdUsuario { get; set; }

    public string? Descripcion { get; set; }

    public DateTime FechaEmitida { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public string RutaServidor { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
