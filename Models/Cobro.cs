using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Cobro
{
    public int IdCobro { get; set; }

    public int IdServicio { get; set; }

    public int IdQr { get; set; }

    public int IdCliente { get; set; }

    public int DiaMesPagar { get; set; }

    public decimal? Monto { get; set; }

    public bool Vigente { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Qr IdQrNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
