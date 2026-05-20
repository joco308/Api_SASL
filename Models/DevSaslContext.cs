using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api_SASL.Models;

public partial class DevSaslContext : DbContext
{
    public DevSaslContext()
    {
    }

    public DevSaslContext(DbContextOptions<DevSaslContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AsignacionEmpleado> AsignacionEmpleados { get; set; }

    public virtual DbSet<AsignacionMaquinarium> AsignacionMaquinaria { get; set; }

    public virtual DbSet<AsignacionRecurso> AsignacionRecursos { get; set; }

    public virtual DbSet<AsignacionUniforme> AsignacionUniformes { get; set; }

    public virtual DbSet<Capacitacione> Capacitaciones { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<DocumentosUsuario> DocumentosUsuarios { get; set; }

    public virtual DbSet<Dominio> Dominios { get; set; }

    public virtual DbSet<EstadoCalidad> EstadoCalidads { get; set; }

    public virtual DbSet<HistorialEstadoMaquina> HistorialEstadoMaquinas { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Incidente> Incidentes { get; set; }

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<MantenimientosMaquinarium> MantenimientosMaquinaria { get; set; }

    public virtual DbSet<Maquinarium> Maquinaria { get; set; }

    public virtual DbSet<MarcaMaquinarium> MarcaMaquinaria { get; set; }

    public virtual DbSet<Memorial> Memorials { get; set; }

    public virtual DbSet<Provedore> Provedores { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServicioTerminado> ServicioTerminados { get; set; }

    public virtual DbSet<SubDominio> SubDominios { get; set; }

    public virtual DbSet<TelefonoCliente> TelefonoClientes { get; set; }

    public virtual DbSet<TelefonoProveedor> TelefonoProveedors { get; set; }

    public virtual DbSet<TelefonoUsuario> TelefonoUsuarios { get; set; }

    public virtual DbSet<Uniforme> Uniformes { get; set; }

    public virtual DbSet<UsuarioTrabajador> UsuarioTrabajadors { get; set; }

    public virtual DbSet<UsuariosCapacitacione> UsuariosCapacitaciones { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AsignacionEmpleado>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdServicio, e.IdHorario });

            entity.ToTable("AsignacionEmpleado");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.DiasLaboralesNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.DiasLaborales)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionEmpleado_DiasLaborales");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionEmpleado_Horario");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionEmpleado_Servicio");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionEmpleado_Usuario");
        });

        modelBuilder.Entity<AsignacionMaquinarium>(entity =>
        {
            entity.HasKey(e => new { e.IdServicio, e.IdMaquinaria });

            entity.ToTable("AsignacionMaquinarium");

            entity.HasOne(d => d.IdMaquinariaNavigation).WithMany(p => p.AsignacionMaquinaria)
                .HasForeignKey(d => d.IdMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionMaquinarium_Maquinaria");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.AsignacionMaquinaria)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionMaquinarium_Servicio");
        });

        modelBuilder.Entity<AsignacionRecurso>(entity =>
        {
            entity.HasKey(e => new { e.IdServicio, e.IdRecurso });

            entity.ToTable("AsignacionRecurso");

            entity.HasOne(d => d.IdRecursoNavigation).WithMany(p => p.AsignacionRecursos)
                .HasForeignKey(d => d.IdRecurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionRecurso_Recurso");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.AsignacionRecursos)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionRecurso_Servicio");
        });

        modelBuilder.Entity<AsignacionUniforme>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionUniforme);

            entity.ToTable("AsignacionUniforme");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdUniformeNavigation).WithMany(p => p.AsignacionUniformes)
                .HasForeignKey(d => d.IdUniforme)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionUniforme_Uniforme");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.AsignacionUniformes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionUniforme_Usuario");
        });

        modelBuilder.Entity<Capacitacione>(entity =>
        {
            entity.HasKey(e => e.IdCapacitacion);

            entity.ToTable("Capacitacione");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("Cliente");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Direccion");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Empresa");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion);

            entity.ToTable("Direccion");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Ncasa).HasColumnName("NCasa");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Direccion_SubDominio_Zona");
        });

        modelBuilder.Entity<DocumentosUsuario>(entity =>
        {
            entity.HasKey(e => e.IdDocumento);

            entity.ToTable("DocumentosUsuario");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NombreArchivo).HasMaxLength(100);
            entity.Property(e => e.UbicacionArchivo).HasMaxLength(200);
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdTipoDeDocumentoNavigation).WithMany(p => p.DocumentosUsuarios)
                .HasForeignKey(d => d.IdTipoDeDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentosUsuario_Tipo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.DocumentosUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentosUsuario_Usuario");
        });

        modelBuilder.Entity<Dominio>(entity =>
        {
            entity.HasKey(e => e.IdDominio);

            entity.ToTable("Dominio");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<EstadoCalidad>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCalidad);

            entity.ToTable("EstadoCalidad");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<HistorialEstadoMaquina>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);

            entity.ToTable("HistorialEstadoMaquina");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdEstadoCalidadNavigation).WithMany(p => p.HistorialEstadoMaquinas)
                .HasForeignKey(d => d.IdEstadoCalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialEstadoMaquina_EstadoCalidad");

            entity.HasOne(d => d.IdMaquinariaNavigation).WithMany(p => p.HistorialEstadoMaquinas)
                .HasForeignKey(d => d.IdMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialEstadoMaquina_Maquinaria");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);

            entity.ToTable("Horario");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<Incidente>(entity =>
        {
            entity.HasKey(e => e.IdIncidente);

            entity.ToTable("Incidente");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Incidentes)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incidente_Servicio");
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.IdMantenimiento);

            entity.ToTable("Mantenimiento");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<MantenimientosMaquinarium>(entity =>
        {
            entity.HasKey(e => new { e.IdMaquinaria, e.IdMantenimiento });

            entity.ToTable("MantenimientosMaquinarium");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdMantenimientoNavigation).WithMany(p => p.MantenimientosMaquinaria)
                .HasForeignKey(d => d.IdMantenimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MantenimientosMaquinarium_Mantenimiento");

            entity.HasOne(d => d.IdMaquinariaNavigation).WithMany(p => p.MantenimientosMaquinaria)
                .HasForeignKey(d => d.IdMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MantenimientosMaquinarium_Maquinaria");
        });

        modelBuilder.Entity<Maquinarium>(entity =>
        {
            entity.HasKey(e => e.IdMaquinaria);

            entity.ToTable("Maquinarium");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdEstadoCalidadNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdEstadoCalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinarium_EstadoCalidad");

            entity.HasOne(d => d.IdMarcaMaquinariaNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdMarcaMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinarium_Marca");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinarium_Proveedor");

            entity.HasOne(d => d.IdTipoMaquinariaNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdTipoMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinarium_TipoMaquinaria");
        });

        modelBuilder.Entity<MarcaMaquinarium>(entity =>
        {
            entity.HasKey(e => e.IdMarcaMaquinaria);

            entity.ToTable("MarcaMaquinarium");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.MarcaMaquinaria)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MarcaMaquinarium_Pais");
        });

        modelBuilder.Entity<Memorial>(entity =>
        {
            entity.HasKey(e => e.IdMemorial);

            entity.ToTable("Memorial");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Memorials)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Memorial_Empleado");
        });

        modelBuilder.Entity<Provedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.ToTable("Provedore");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ProvedoreIdEmpresaNavigations)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provedore_Empresa");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProvedoreIdProductoNavigations)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provedore_Producto");
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.IdRecurso);

            entity.ToTable("Recurso");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Recursos)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recurso_Proveedor");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Recursos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recurso_Tipo");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("Role");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio);

            entity.ToTable("Servicio");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_Cliente");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_Direccion");

            entity.HasOne(d => d.TipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_TipoServicio");
        });

        modelBuilder.Entity<ServicioTerminado>(entity =>
        {
            entity.HasKey(e => e.IdServicioTerminado);

            entity.ToTable("ServicioTerminado");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ServicioTerminados)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioTerminado_Servicio");

            entity.HasOne(d => d.SatisfaccionNavigation).WithMany(p => p.ServicioTerminados)
                .HasForeignKey(d => d.Satisfaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioTerminado_Satisfaccion");
        });

        modelBuilder.Entity<SubDominio>(entity =>
        {
            entity.HasKey(e => e.IdSubDominio);

            entity.ToTable("SubDominio");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdDominioNavigation).WithMany(p => p.SubDominios)
                .HasForeignKey(d => d.IdDominio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubDominio_Dominio");
        });

        modelBuilder.Entity<TelefonoCliente>(entity =>
        {
            entity.HasKey(e => e.IdTelefono);

            entity.ToTable("TelefonoCliente");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.TelefonoClientes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonoCliente_Cliente");

            entity.HasOne(d => d.IdDetalleNavigation).WithMany(p => p.TelefonoClientes)
                .HasForeignKey(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonoCliente_Detalle");
        });

        modelBuilder.Entity<TelefonoProveedor>(entity =>
        {
            entity.HasKey(e => e.IdTelefono);

            entity.ToTable("TelefonoProveedor");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdDetalleNavigation).WithMany(p => p.TelefonoProveedors)
                .HasForeignKey(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonoProveedor_Detalle");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.TelefonoProveedors)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonoProveedor_Proveedor");
        });

        modelBuilder.Entity<TelefonoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTelefonoUsuario);

            entity.ToTable("TelefonoUsuario");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdDetalleNavigation).WithMany(p => p.TelefonoUsuarios)
                .HasForeignKey(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonoUsuario_Detalle");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TelefonoUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelefonoUsuario_Usuario");
        });

        modelBuilder.Entity<Uniforme>(entity =>
        {
            entity.HasKey(e => e.IdUniforme);

            entity.ToTable("Uniforme");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<UsuarioTrabajador>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("UsuarioTrabajador");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UpdateAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.UsuarioTrabajadors)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioTrabajador_Direccion");

            entity.HasOne(d => d.IdEstadoCivilNavigation).WithMany(p => p.UsuarioTrabajadorIdEstadoCivilNavigations)
                .HasForeignKey(d => d.IdEstadoCivil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioTrabajador_EstadoCivil");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.UsuarioTrabajadorIdGeneroNavigations)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioTrabajador_Genero");

            entity.HasOne(d => d.IdGradoAcademicoNavigation).WithMany(p => p.UsuarioTrabajadorIdGradoAcademicoNavigations)
                .HasForeignKey(d => d.IdGradoAcademico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioTrabajador_GradoAcademico");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.UsuarioTrabajadorIdPaisNavigations)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioTrabajador_Pais");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioTrabajadors)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioTrabajador_Role");

            entity.HasMany(d => d.IdSubDominios).WithMany(p => p.IdUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioTrabajadorCarrera",
                    r => r.HasOne<SubDominio>().WithMany()
                        .HasForeignKey("IdSubDominio")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UTCarrera_SubDominio"),
                    l => l.HasOne<UsuarioTrabajador>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UTCarrera_Usuario"),
                    j =>
                    {
                        j.HasKey("IdUsuario", "IdSubDominio");
                        j.ToTable("UsuarioTrabajador_Carrera");
                    });
        });

        modelBuilder.Entity<UsuariosCapacitacione>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdCapacitacion });

            entity.ToTable("UsuariosCapacitacione");

            entity.HasOne(d => d.IdCapacitacionNavigation).WithMany(p => p.UsuariosCapacitaciones)
                .HasForeignKey(d => d.IdCapacitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuariosCapacitacione_Capacitacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosCapacitaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuariosCapacitacione_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
