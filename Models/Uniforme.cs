using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Uniforme
{
    public int IdUniforme { get; set; }

    public string NombreUniforme { get; set; } = null!;

    public int Talla { get; set; }

    public string? Descripcion { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionUniforme> AsignacionUniformes { get; set; } = new List<AsignacionUniforme>();
}
