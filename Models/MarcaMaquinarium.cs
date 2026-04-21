using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class MarcaMaquinarium
{
    public int IdMarcaMaquinaria { get; set; }

    public int IdPais { get; set; }

    public string NombreMarca { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual SubDominio IdPaisNavigation { get; set; } = null!;

    public virtual ICollection<Maquinarium> Maquinaria { get; set; } = new List<Maquinarium>();
}
