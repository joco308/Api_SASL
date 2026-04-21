using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class DocumentosUsuario
{
    public int IdDocumento { get; set; }

    public int IdUsuario { get; set; }

    public string TipoDeDocumento { get; set; } = null!;

    public string Archivo { get; set; } = null!;

    public DateOnly FechaSubida { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
