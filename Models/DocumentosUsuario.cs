using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class DocumentosUsuario
{
    public int IdDocumento { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoDeDocumento { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public DateOnly FechaSubida { get; set; }

    public string UbicacionArchivo { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual SubDominio IdTipoDeDocumentoNavigation { get; set; } = null!;

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
