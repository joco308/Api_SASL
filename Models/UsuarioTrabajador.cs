using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class UsuarioTrabajador
{
    public int IdUsuario { get; set; }

    public int IdEstadoCivil { get; set; }

    public int IdGradoAcademico { get; set; }

    public int IdGenero { get; set; }

    public int IdDireccion { get; set; }

    public int IdRol { get; set; }

    public int IdPais { get; set; }

    public string ContrasenaHash { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int Ci { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionEmpleado> AsignacionEmpleados { get; set; } = new List<AsignacionEmpleado>();

    public virtual ICollection<AsignacionUniforme> AsignacionUniformes { get; set; } = new List<AsignacionUniforme>();

    public virtual ICollection<DocumentosUsuario> DocumentosUsuarios { get; set; } = new List<DocumentosUsuario>();

    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    public virtual SubDominio IdEstadoCivilNavigation { get; set; } = null!;

    public virtual SubDominio IdGeneroNavigation { get; set; } = null!;

    public virtual SubDominio IdGradoAcademicoNavigation { get; set; } = null!;

    public virtual SubDominio IdPaisNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Memorial> Memorials { get; set; } = new List<Memorial>();

    public virtual ICollection<TelefonoUsuario> TelefonoUsuarios { get; set; } = new List<TelefonoUsuario>();

    public virtual ICollection<UsuariosCapacitacione> UsuariosCapacitaciones { get; set; } = new List<UsuariosCapacitacione>();

    public virtual ICollection<SubDominio> IdCarreras { get; set; } = new List<SubDominio>();
}
