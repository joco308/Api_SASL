using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Dominio
{
    public int IdDominio { get; set; }

    public string Dominio1 { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<SubDominio> SubDominios { get; set; } = new List<SubDominio>();
}
