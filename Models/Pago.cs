using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdCobro { get; set; }

    public DateTime FechaPago { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Cobro IdCobroNavigation { get; set; } = null!;
}
