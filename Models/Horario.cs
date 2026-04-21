using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public TimeOnly HoraEntrada { get; set; }

    public TimeOnly HoraSalida { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionEmpleado> AsignacionEmpleados { get; set; } = new List<AsignacionEmpleado>();
}
