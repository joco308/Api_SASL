using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class AsignacionEmpleado
{
    public int IdUsuario { get; set; }

    public int IdServicio { get; set; }

    public int IdHorario { get; set; }

    public int DiasLaborales { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual SubDominio DiasLaboralesNavigation { get; set; } = null!;

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual UsuarioTrabajador IdUsuarioNavigation { get; set; } = null!;
}
