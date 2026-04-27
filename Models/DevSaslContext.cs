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
            entity.HasKey(e => new { e.IdUsuario, e.IdServicio });

            entity.ToTable("Asignacion_empleados");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.DiasLaborales).HasColumnName("dias_laborales");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.DiasLaboralesNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.DiasLaborales)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_empleados_dias");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_empleados_horario");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_empleados_servicio");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.AsignacionEmpleados)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_empleados_usuario");
        });

        modelBuilder.Entity<AsignacionMaquinarium>(entity =>
        {
            entity.HasKey(e => new { e.IdServicio, e.IdMaquinaria });

            entity.ToTable("Asignacion_maquinaria");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.IdMaquinaria).HasColumnName("id_maquinaria");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");

            entity.HasOne(d => d.IdMaquinariaNavigation).WithMany(p => p.AsignacionMaquinaria)
                .HasForeignKey(d => d.IdMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_maquinaria_maquinaria");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.AsignacionMaquinaria)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_maquinaria_servicio");
        });

        modelBuilder.Entity<AsignacionRecurso>(entity =>
        {
            entity.HasKey(e => new { e.IdServicio, e.IdRecurso });

            entity.ToTable("Asignacion_recursos");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.IdRecurso).HasColumnName("id_recurso");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");

            entity.HasOne(d => d.IdRecursoNavigation).WithMany(p => p.AsignacionRecursos)
                .HasForeignKey(d => d.IdRecurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_recursos_recurso");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.AsignacionRecursos)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_recursos_servicio");
        });

        modelBuilder.Entity<AsignacionUniforme>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionUniforme);

            entity.ToTable("Asignacion_uniformes");

            entity.Property(e => e.IdAsignacionUniforme).HasColumnName("id_asignacion_uniforme");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.FechaDevolucion).HasColumnName("fecha_devolucion");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.IdUniforme).HasColumnName("id_uniforme");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdUniformeNavigation).WithMany(p => p.AsignacionUniformes)
                .HasForeignKey(d => d.IdUniforme)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_uniformes_uniforme");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.AsignacionUniformes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_uniformes_usuario");
        });

        modelBuilder.Entity<Capacitacione>(entity =>
        {
            entity.HasKey(e => e.IdCapacitacion);

            entity.Property(e => e.IdCapacitacion).HasColumnName("id_capacitacion");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("Cliente");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.ContactoEmergencia)
                .HasMaxLength(50)
                .HasColumnName("contacto_emergencia");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .HasColumnName("nombre_cliente");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_direccion");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_empresa");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion);

            entity.ToTable("Direccion");

            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .HasColumnName("calle");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdZona).HasColumnName("id_zona");
            entity.Property(e => e.NCasa).HasColumnName("n_casa");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Direccion_id_zona");
        });

        modelBuilder.Entity<DocumentosUsuario>(entity =>
        {
            entity.HasKey(e => e.IdDocumento);

            entity.ToTable("Documentos_usuarios");

            entity.Property(e => e.IdDocumento).HasColumnName("id_documento");
            entity.Property(e => e.Archivo).HasColumnName("archivo");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.FechaSubida).HasColumnName("fecha_subida");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.TipoDeDocumento)
                .HasMaxLength(100)
                .HasColumnName("tipo_de_documento");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.DocumentosUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documentos_usuarios_usuario");
        });

        modelBuilder.Entity<Dominio>(entity =>
        {
            entity.HasKey(e => e.IdDominio);

            entity.Property(e => e.IdDominio).HasColumnName("id_dominio");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Dominio1)
                .HasMaxLength(100)
                .HasColumnName("dominio");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<EstadoCalidad>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCalidad);

            entity.ToTable("Estado_calidad");

            entity.Property(e => e.IdEstadoCalidad).HasColumnName("id_estado_calidad");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.EstadoCalidad1)
                .HasMaxLength(100)
                .HasColumnName("estado_calidad");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<HistorialEstadoMaquina>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);

            entity.ToTable("Historial_estado_maquina");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCambio).HasColumnName("fecha_cambio");
            entity.Property(e => e.IdEstadoCalidad).HasColumnName("id_estado_calidad");
            entity.Property(e => e.IdMaquinaria).HasColumnName("id_maquinaria");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdEstadoCalidadNavigation).WithMany(p => p.HistorialEstadoMaquinas)
                .HasForeignKey(d => d.IdEstadoCalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_estado_maquina_estado");

            entity.HasOne(d => d.IdMaquinariaNavigation).WithMany(p => p.HistorialEstadoMaquinas)
                .HasForeignKey(d => d.IdMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_estado_maquina_maquinaria");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);

            entity.ToTable("Horario");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");
            entity.Property(e => e.HoraSalida).HasColumnName("hora_salida");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<Incidente>(entity =>
        {
            entity.HasKey(e => e.IdIncidente);

            entity.Property(e => e.IdIncidente).HasColumnName("id_incidente");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Incidentes)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incidentes_servicio");
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.IdMantenimiento);

            entity.ToTable("Mantenimiento");

            entity.Property(e => e.IdMantenimiento).HasColumnName("id_mantenimiento");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaMantenimiento).HasColumnName("fecha_mantenimiento");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<MantenimientosMaquinarium>(entity =>
        {
            entity.HasKey(e => new { e.IdMaquinaria, e.IdMantenimiento });

            entity.ToTable("Mantenimientos_maquinaria");

            entity.Property(e => e.IdMaquinaria).HasColumnName("id_maquinaria");
            entity.Property(e => e.IdMantenimiento).HasColumnName("id_mantenimiento");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdMantenimientoNavigation).WithMany(p => p.MantenimientosMaquinaria)
                .HasForeignKey(d => d.IdMantenimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mantenimientos_maquinaria_mantenimiento");

            entity.HasOne(d => d.IdMaquinariaNavigation).WithMany(p => p.MantenimientosMaquinaria)
                .HasForeignKey(d => d.IdMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mantenimientos_maquinaria_maquinaria");
        });

        modelBuilder.Entity<Maquinarium>(entity =>
        {
            entity.HasKey(e => e.IdMaquinaria);

            entity.Property(e => e.IdMaquinaria).HasColumnName("id_maquinaria");
            entity.Property(e => e.CodigoInv)
                .HasMaxLength(50)
                .HasColumnName("codigo_inv");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdEstadoCalidad).HasColumnName("id_estado_calidad");
            entity.Property(e => e.IdMarcaMaquinaria).HasColumnName("id_marca_maquinaria");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.IdTipoMaquinaria).HasColumnName("id_tipo_maquinaria");
            entity.Property(e => e.NombreMaquinaria)
                .HasMaxLength(100)
                .HasColumnName("nombre_maquinaria");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdEstadoCalidadNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdEstadoCalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinaria_estado_calidad");

            entity.HasOne(d => d.IdMarcaMaquinariaNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdMarcaMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinaria_marca");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinaria_proveedor");

            entity.HasOne(d => d.IdTipoMaquinariaNavigation).WithMany(p => p.Maquinaria)
                .HasForeignKey(d => d.IdTipoMaquinaria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maquinaria_tipo");
        });

        modelBuilder.Entity<MarcaMaquinarium>(entity =>
        {
            entity.HasKey(e => e.IdMarcaMaquinaria);

            entity.ToTable("Marca_maquinaria");

            entity.Property(e => e.IdMarcaMaquinaria).HasColumnName("id_marca_maquinaria");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.NombreMarca)
                .HasMaxLength(100)
                .HasColumnName("nombre_marca");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.MarcaMaquinaria)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marca_maquinaria_pais");
        });

        modelBuilder.Entity<Memorial>(entity =>
        {
            entity.HasKey(e => e.IdMemorial);

            entity.ToTable("Memorial");

            entity.Property(e => e.IdMemorial).HasColumnName("id_memorial");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Memorials)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Memorial_empleado");
        });

        modelBuilder.Entity<Provedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Nit).HasColumnName("nit");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ProvedoreIdEmpresaNavigations)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provedores_empresa");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProvedoreIdProductoNavigations)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provedores_producto");
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.IdRecurso);

            entity.Property(e => e.IdRecurso).HasColumnName("id_recurso");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Recursos)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recursos_proveedor");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Recursos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recursos_tipo");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
            entity.Property(e => e.Salario).HasColumnName("salario");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio);

            entity.ToTable("Servicio");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.TipoServicio).HasColumnName("tipo_servicio");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_cliente");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_direccion");

            entity.HasOne(d => d.TipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_tipo_servicio");
        });

        modelBuilder.Entity<ServicioTerminado>(entity =>
        {
            entity.HasKey(e => e.IdServicioTerminado);

            entity.ToTable("Servicio_terminado");

            entity.Property(e => e.IdServicioTerminado).HasColumnName("id_servicio_terminado");
            entity.Property(e => e.Comentarios)
                .HasMaxLength(300)
                .HasColumnName("comentarios");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Satisfaccion).HasColumnName("satisfaccion");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ServicioTerminados)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_terminado_servicio");

            entity.HasOne(d => d.SatisfaccionNavigation).WithMany(p => p.ServicioTerminados)
                .HasForeignKey(d => d.Satisfaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_terminado_satisfaccion");
        });

        modelBuilder.Entity<SubDominio>(entity =>
        {
            entity.HasKey(e => e.IdSubDominio);

            entity.ToTable("Sub_dominios");

            entity.Property(e => e.IdSubDominio).HasColumnName("id_sub_dominio");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Detalle)
                .HasMaxLength(100)
                .HasColumnName("detalle");
            entity.Property(e => e.IdDominio).HasColumnName("id_dominio");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdDominioNavigation).WithMany(p => p.SubDominios)
                .HasForeignKey(d => d.IdDominio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_dominios_id_dominio");

            entity.HasMany(d => d.IdUsuarios).WithMany(p => p.IdCarreras)
                .UsingEntity<Dictionary<string, object>>(
                    "CarrerasUsuario",
                    r => r.HasOne<UsuarioTrabajador>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Carreras_usuario_usuario"),
                    l => l.HasOne<SubDominio>().WithMany()
                        .HasForeignKey("IdCarrera")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Carreras_usuario_carrera"),
                    j =>
                    {
                        j.HasKey("IdCarrera", "IdUsuario");
                        j.ToTable("Carreras_usuario");
                        j.IndexerProperty<int>("IdCarrera").HasColumnName("id_carrera");
                        j.IndexerProperty<int>("IdUsuario").HasColumnName("id_usuario");
                    });
        });

        modelBuilder.Entity<TelefonoCliente>(entity =>
        {
            entity.HasKey(e => e.IdTelefono);

            entity.ToTable("Telefono_cliente");

            entity.Property(e => e.IdTelefono).HasColumnName("id_telefono");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.TelefonoClientes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_cliente_cliente");

            entity.HasOne(d => d.IdDetalleNavigation).WithMany(p => p.TelefonoClientes)
                .HasForeignKey(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_cliente_detalle");
        });

        modelBuilder.Entity<TelefonoProveedor>(entity =>
        {
            entity.HasKey(e => e.IdTelefono);

            entity.ToTable("Telefono_proveedor");

            entity.Property(e => e.IdTelefono).HasColumnName("id_telefono");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdDetalleNavigation).WithMany(p => p.TelefonoProveedors)
                .HasForeignKey(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_proveedor_detalle");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.TelefonoProveedors)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_proveedor_proveedor");
        });

        modelBuilder.Entity<TelefonoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTelefonoUsuario);

            entity.ToTable("Telefono_usuarios");

            entity.Property(e => e.IdTelefonoUsuario).HasColumnName("id_telefono_usuario");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.TelefonoUsuario1).HasColumnName("telefono_usuario");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdDetalleNavigation).WithMany(p => p.TelefonoUsuarios)
                .HasForeignKey(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_usuarios_detalle");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TelefonoUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_usuarios_usuario");
        });

        modelBuilder.Entity<Uniforme>(entity =>
        {
            entity.HasKey(e => e.IdUniforme);

            entity.Property(e => e.IdUniforme).HasColumnName("id_uniforme");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .HasColumnName("descripcion");
            entity.Property(e => e.NombreUniforme)
                .HasMaxLength(100)
                .HasColumnName("nombre_uniforme");
            entity.Property(e => e.Talla).HasColumnName("talla");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<UsuarioTrabajador>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario_trabajador");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Ci).HasColumnName("ci");
            entity.Property(e => e.Codigo2fa)
                .HasMaxLength(6)
                .HasColumnName("codigo2fa");
            entity.Property(e => e.ContrasenaHash)
                .HasMaxLength(60)
                .HasColumnName("contrasena_hash");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("create_at");
            entity.Property(e => e.Expiracion).HasColumnName("expiracion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.IdEstadoCivil).HasColumnName("id_estado_civil");
            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.IdGradoAcademico).HasColumnName("id_grado_academico");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Pediente2fa).HasColumnName("pediente_2fa");
            entity.Property(e => e.ServicioAsignado).HasColumnName("servicio_asignado");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.UsuarioTrabajadors)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_trabajador_direccion");

            entity.HasOne(d => d.IdEstadoCivilNavigation).WithMany(p => p.UsuarioTrabajadorIdEstadoCivilNavigations)
                .HasForeignKey(d => d.IdEstadoCivil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_trabajador_estado_civil");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.UsuarioTrabajadorIdGeneroNavigations)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_trabajador_genero");

            entity.HasOne(d => d.IdGradoAcademicoNavigation).WithMany(p => p.UsuarioTrabajadorIdGradoAcademicoNavigations)
                .HasForeignKey(d => d.IdGradoAcademico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_trabajador_grado_academico");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.UsuarioTrabajadorIdPaisNavigations)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_trabajador_pais");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioTrabajadors)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_trabajador_rol");
        });

        modelBuilder.Entity<UsuariosCapacitacione>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdCapacitacion });

            entity.ToTable("Usuarios_capacitaciones");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdCapacitacion).HasColumnName("id_capacitacion");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");

            entity.HasOne(d => d.IdCapacitacionNavigation).WithMany(p => p.UsuariosCapacitaciones)
                .HasForeignKey(d => d.IdCapacitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_capacitaciones_capacitacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosCapacitaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_capacitaciones_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
