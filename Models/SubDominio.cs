using System;
using System.Collections.Generic;

namespace Api_SASL.Models;

public partial class SubDominio
{
    public int IdSubDominio { get; set; }

    public int IdDominio { get; set; }

    public string Detalle { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<AsignacionEmpleado> AsignacionEmpleados { get; set; } = new List<AsignacionEmpleado>();

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();

    public virtual Dominio IdDominioNavigation { get; set; } = null!;

    public virtual ICollection<Maquinarium> Maquinaria { get; set; } = new List<Maquinarium>();

    public virtual ICollection<MarcaMaquinarium> MarcaMaquinaria { get; set; } = new List<MarcaMaquinarium>();

    public virtual ICollection<Provedore> ProvedoreIdEmpresaNavigations { get; set; } = new List<Provedore>();

    public virtual ICollection<Provedore> ProvedoreIdProductoNavigations { get; set; } = new List<Provedore>();

    public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();

    public virtual ICollection<ServicioTerminado> ServicioTerminados { get; set; } = new List<ServicioTerminado>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<TelefonoCliente> TelefonoClientes { get; set; } = new List<TelefonoCliente>();

    public virtual ICollection<TelefonoProveedor> TelefonoProveedors { get; set; } = new List<TelefonoProveedor>();

    public virtual ICollection<TelefonoUsuario> TelefonoUsuarios { get; set; } = new List<TelefonoUsuario>();

    public virtual ICollection<UsuarioTrabajador> UsuarioTrabajadorIdEstadoCivilNavigations { get; set; } = new List<UsuarioTrabajador>();

    public virtual ICollection<UsuarioTrabajador> UsuarioTrabajadorIdGeneroNavigations { get; set; } = new List<UsuarioTrabajador>();

    public virtual ICollection<UsuarioTrabajador> UsuarioTrabajadorIdGradoAcademicoNavigations { get; set; } = new List<UsuarioTrabajador>();

    public virtual ICollection<UsuarioTrabajador> UsuarioTrabajadorIdPaisNavigations { get; set; } = new List<UsuarioTrabajador>();

    public virtual ICollection<UsuarioTrabajador> IdUsuarios { get; set; } = new List<UsuarioTrabajador>();
}
