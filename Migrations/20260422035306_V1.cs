using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_SASL.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Capacitaciones",
                columns: table => new
                {
                    id_capacitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capacitaciones", x => x.id_capacitacion);
                });

            migrationBuilder.CreateTable(
                name: "Dominios",
                columns: table => new
                {
                    id_dominio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dominio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dominios", x => x.id_dominio);
                });

            migrationBuilder.CreateTable(
                name: "Estado_calidad",
                columns: table => new
                {
                    id_estado_calidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    estado_calidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado_calidad", x => x.id_estado_calidad);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    id_horario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hora_entrada = table.Column<TimeOnly>(type: "time", nullable: false),
                    hora_salida = table.Column<TimeOnly>(type: "time", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.id_horario);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimiento",
                columns: table => new
                {
                    id_mantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_mantenimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    costo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimiento", x => x.id_mantenimiento);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    salario = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Uniformes",
                columns: table => new
                {
                    id_uniforme = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_uniforme = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    talla = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uniformes", x => x.id_uniforme);
                });

            migrationBuilder.CreateTable(
                name: "Sub_dominios",
                columns: table => new
                {
                    id_sub_dominio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_dominio = table.Column<int>(type: "int", nullable: false),
                    detalle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sub_dominios", x => x.id_sub_dominio);
                    table.ForeignKey(
                        name: "FK_Sub_dominios_id_dominio",
                        column: x => x.id_dominio,
                        principalTable: "Dominios",
                        principalColumn: "id_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Direccion",
                columns: table => new
                {
                    id_direccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_zona = table.Column<int>(type: "int", nullable: false),
                    calle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    n_casa = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direccion", x => x.id_direccion);
                    table.ForeignKey(
                        name: "FK_Direccion_id_zona",
                        column: x => x.id_zona,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Marca_maquinaria",
                columns: table => new
                {
                    id_marca_maquinaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_pais = table.Column<int>(type: "int", nullable: false),
                    nombre_marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marca_maquinaria", x => x.id_marca_maquinaria);
                    table.ForeignKey(
                        name: "FK_Marca_maquinaria_pais",
                        column: x => x.id_pais,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Provedores",
                columns: table => new
                {
                    id_proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_empresa = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    nit = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provedores", x => x.id_proveedor);
                    table.ForeignKey(
                        name: "FK_Provedores_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Provedores_producto",
                        column: x => x.id_producto,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_empresa = table.Column<int>(type: "int", nullable: false),
                    id_direccion = table.Column<int>(type: "int", nullable: false),
                    nombre_cliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contacto_emergencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.id_cliente);
                    table.ForeignKey(
                        name: "FK_Cliente_direccion",
                        column: x => x.id_direccion,
                        principalTable: "Direccion",
                        principalColumn: "id_direccion");
                    table.ForeignKey(
                        name: "FK_Cliente_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Usuario_trabajador",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estado_civil = table.Column<int>(type: "int", nullable: false),
                    id_grado_academico = table.Column<int>(type: "int", nullable: false),
                    id_genero = table.Column<int>(type: "int", nullable: false),
                    id_direccion = table.Column<int>(type: "int", nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_pais = table.Column<int>(type: "int", nullable: false),
                    contrasena_hash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ci = table.Column<int>(type: "int", nullable: false),
                    nombre_usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    codigo2fa = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    expiracion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    segundo_factor_pendiente = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario_trabajador", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuario_trabajador_direccion",
                        column: x => x.id_direccion,
                        principalTable: "Direccion",
                        principalColumn: "id_direccion");
                    table.ForeignKey(
                        name: "FK_Usuario_trabajador_estado_civil",
                        column: x => x.id_estado_civil,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Usuario_trabajador_genero",
                        column: x => x.id_genero,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Usuario_trabajador_grado_academico",
                        column: x => x.id_grado_academico,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Usuario_trabajador_pais",
                        column: x => x.id_pais,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Usuario_trabajador_rol",
                        column: x => x.id_rol,
                        principalTable: "Roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Maquinaria",
                columns: table => new
                {
                    id_maquinaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_proveedor = table.Column<int>(type: "int", nullable: false),
                    id_tipo_maquinaria = table.Column<int>(type: "int", nullable: false),
                    id_estado_calidad = table.Column<int>(type: "int", nullable: false),
                    id_marca_maquinaria = table.Column<int>(type: "int", nullable: false),
                    nombre_maquinaria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    codigo_inv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinaria", x => x.id_maquinaria);
                    table.ForeignKey(
                        name: "FK_Maquinaria_estado_calidad",
                        column: x => x.id_estado_calidad,
                        principalTable: "Estado_calidad",
                        principalColumn: "id_estado_calidad");
                    table.ForeignKey(
                        name: "FK_Maquinaria_marca",
                        column: x => x.id_marca_maquinaria,
                        principalTable: "Marca_maquinaria",
                        principalColumn: "id_marca_maquinaria");
                    table.ForeignKey(
                        name: "FK_Maquinaria_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "Provedores",
                        principalColumn: "id_proveedor");
                    table.ForeignKey(
                        name: "FK_Maquinaria_tipo",
                        column: x => x.id_tipo_maquinaria,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    id_recurso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_proveedor = table.Column<int>(type: "int", nullable: false),
                    id_tipo = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.id_recurso);
                    table.ForeignKey(
                        name: "FK_Recursos_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "Provedores",
                        principalColumn: "id_proveedor");
                    table.ForeignKey(
                        name: "FK_Recursos_tipo",
                        column: x => x.id_tipo,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Telefono_proveedor",
                columns: table => new
                {
                    id_telefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telefono = table.Column<int>(type: "int", nullable: false),
                    id_detalle = table.Column<int>(type: "int", nullable: false),
                    id_proveedor = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefono_proveedor", x => x.id_telefono);
                    table.ForeignKey(
                        name: "FK_Telefono_proveedor_detalle",
                        column: x => x.id_detalle,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Telefono_proveedor_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "Provedores",
                        principalColumn: "id_proveedor");
                });

            migrationBuilder.CreateTable(
                name: "Servicio",
                columns: table => new
                {
                    id_servicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_direccion = table.Column<int>(type: "int", nullable: false),
                    tipo_servicio = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_final = table.Column<DateOnly>(type: "date", nullable: true),
                    costo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio", x => x.id_servicio);
                    table.ForeignKey(
                        name: "FK_Servicio_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "FK_Servicio_direccion",
                        column: x => x.id_direccion,
                        principalTable: "Direccion",
                        principalColumn: "id_direccion");
                    table.ForeignKey(
                        name: "FK_Servicio_tipo_servicio",
                        column: x => x.tipo_servicio,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Telefono_cliente",
                columns: table => new
                {
                    id_telefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telefono = table.Column<int>(type: "int", nullable: false),
                    id_detalle = table.Column<int>(type: "int", nullable: false),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefono_cliente", x => x.id_telefono);
                    table.ForeignKey(
                        name: "FK_Telefono_cliente_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "FK_Telefono_cliente_detalle",
                        column: x => x.id_detalle,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                });

            migrationBuilder.CreateTable(
                name: "Asignacion_uniformes",
                columns: table => new
                {
                    id_asignacion_uniforme = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_uniforme = table.Column<int>(type: "int", nullable: false),
                    fecha_entrega = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_devolucion = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion_uniformes", x => x.id_asignacion_uniforme);
                    table.ForeignKey(
                        name: "FK_Asignacion_uniformes_uniforme",
                        column: x => x.id_uniforme,
                        principalTable: "Uniformes",
                        principalColumn: "id_uniforme");
                    table.ForeignKey(
                        name: "FK_Asignacion_uniformes_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Carreras_usuario",
                columns: table => new
                {
                    id_carrera = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras_usuario", x => new { x.id_carrera, x.id_usuario });
                    table.ForeignKey(
                        name: "FK_Carreras_usuario_carrera",
                        column: x => x.id_carrera,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Carreras_usuario_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Documentos_usuarios",
                columns: table => new
                {
                    id_documento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    tipo_de_documento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    archivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha_subida = table.Column<DateOnly>(type: "date", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos_usuarios", x => x.id_documento);
                    table.ForeignKey(
                        name: "FK_Documentos_usuarios_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Memorial",
                columns: table => new
                {
                    id_memorial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_empleado = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memorial", x => x.id_memorial);
                    table.ForeignKey(
                        name: "FK_Memorial_empleado",
                        column: x => x.id_empleado,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Telefono_usuarios",
                columns: table => new
                {
                    id_telefono_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telefono_usuario = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_detalle = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefono_usuarios", x => x.id_telefono_usuario);
                    table.ForeignKey(
                        name: "FK_Telefono_usuarios_detalle",
                        column: x => x.id_detalle,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Telefono_usuarios_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios_capacitaciones",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_capacitacion = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios_capacitaciones", x => new { x.id_usuario, x.id_capacitacion });
                    table.ForeignKey(
                        name: "FK_Usuarios_capacitaciones_capacitacion",
                        column: x => x.id_capacitacion,
                        principalTable: "Capacitaciones",
                        principalColumn: "id_capacitacion");
                    table.ForeignKey(
                        name: "FK_Usuarios_capacitaciones_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Historial_estado_maquina",
                columns: table => new
                {
                    id_historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_maquinaria = table.Column<int>(type: "int", nullable: false),
                    id_estado_calidad = table.Column<int>(type: "int", nullable: false),
                    fecha_cambio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historial_estado_maquina", x => x.id_historial);
                    table.ForeignKey(
                        name: "FK_Historial_estado_maquina_estado",
                        column: x => x.id_estado_calidad,
                        principalTable: "Estado_calidad",
                        principalColumn: "id_estado_calidad");
                    table.ForeignKey(
                        name: "FK_Historial_estado_maquina_maquinaria",
                        column: x => x.id_maquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "id_maquinaria");
                });

            migrationBuilder.CreateTable(
                name: "Mantenimientos_maquinaria",
                columns: table => new
                {
                    id_maquinaria = table.Column<int>(type: "int", nullable: false),
                    id_mantenimiento = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimientos_maquinaria", x => new { x.id_maquinaria, x.id_mantenimiento });
                    table.ForeignKey(
                        name: "FK_Mantenimientos_maquinaria_mantenimiento",
                        column: x => x.id_mantenimiento,
                        principalTable: "Mantenimiento",
                        principalColumn: "id_mantenimiento");
                    table.ForeignKey(
                        name: "FK_Mantenimientos_maquinaria_maquinaria",
                        column: x => x.id_maquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "id_maquinaria");
                });

            migrationBuilder.CreateTable(
                name: "Asignacion_empleados",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_servicio = table.Column<int>(type: "int", nullable: false),
                    id_horario = table.Column<int>(type: "int", nullable: false),
                    dias_laborales = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion_empleados", x => new { x.id_usuario, x.id_servicio });
                    table.ForeignKey(
                        name: "FK_Asignacion_empleados_dias",
                        column: x => x.dias_laborales,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Asignacion_empleados_horario",
                        column: x => x.id_horario,
                        principalTable: "Horario",
                        principalColumn: "id_horario");
                    table.ForeignKey(
                        name: "FK_Asignacion_empleados_servicio",
                        column: x => x.id_servicio,
                        principalTable: "Servicio",
                        principalColumn: "id_servicio");
                    table.ForeignKey(
                        name: "FK_Asignacion_empleados_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario_trabajador",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Asignacion_maquinaria",
                columns: table => new
                {
                    id_servicio = table.Column<int>(type: "int", nullable: false),
                    id_maquinaria = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion_maquinaria", x => new { x.id_servicio, x.id_maquinaria });
                    table.ForeignKey(
                        name: "FK_Asignacion_maquinaria_maquinaria",
                        column: x => x.id_maquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "id_maquinaria");
                    table.ForeignKey(
                        name: "FK_Asignacion_maquinaria_servicio",
                        column: x => x.id_servicio,
                        principalTable: "Servicio",
                        principalColumn: "id_servicio");
                });

            migrationBuilder.CreateTable(
                name: "Asignacion_recursos",
                columns: table => new
                {
                    id_servicio = table.Column<int>(type: "int", nullable: false),
                    id_recurso = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion_recursos", x => new { x.id_servicio, x.id_recurso });
                    table.ForeignKey(
                        name: "FK_Asignacion_recursos_recurso",
                        column: x => x.id_recurso,
                        principalTable: "Recursos",
                        principalColumn: "id_recurso");
                    table.ForeignKey(
                        name: "FK_Asignacion_recursos_servicio",
                        column: x => x.id_servicio,
                        principalTable: "Servicio",
                        principalColumn: "id_servicio");
                });

            migrationBuilder.CreateTable(
                name: "Incidentes",
                columns: table => new
                {
                    id_incidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_servicio = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidentes", x => x.id_incidente);
                    table.ForeignKey(
                        name: "FK_Incidentes_servicio",
                        column: x => x.id_servicio,
                        principalTable: "Servicio",
                        principalColumn: "id_servicio");
                });

            migrationBuilder.CreateTable(
                name: "Servicio_terminado",
                columns: table => new
                {
                    id_servicio_terminado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_servicio = table.Column<int>(type: "int", nullable: false),
                    satisfaccion = table.Column<int>(type: "int", nullable: false),
                    comentarios = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio_terminado", x => x.id_servicio_terminado);
                    table.ForeignKey(
                        name: "FK_Servicio_terminado_satisfaccion",
                        column: x => x.satisfaccion,
                        principalTable: "Sub_dominios",
                        principalColumn: "id_sub_dominio");
                    table.ForeignKey(
                        name: "FK_Servicio_terminado_servicio",
                        column: x => x.id_servicio,
                        principalTable: "Servicio",
                        principalColumn: "id_servicio");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_empleados_dias_laborales",
                table: "Asignacion_empleados",
                column: "dias_laborales");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_empleados_id_horario",
                table: "Asignacion_empleados",
                column: "id_horario");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_empleados_id_servicio",
                table: "Asignacion_empleados",
                column: "id_servicio");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_maquinaria_id_maquinaria",
                table: "Asignacion_maquinaria",
                column: "id_maquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_recursos_id_recurso",
                table: "Asignacion_recursos",
                column: "id_recurso");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_uniformes_id_uniforme",
                table: "Asignacion_uniformes",
                column: "id_uniforme");

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_uniformes_id_usuario",
                table: "Asignacion_uniformes",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Carreras_usuario_id_usuario",
                table: "Carreras_usuario",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_id_direccion",
                table: "Cliente",
                column: "id_direccion");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_id_empresa",
                table: "Cliente",
                column: "id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Direccion_id_zona",
                table: "Direccion",
                column: "id_zona");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_usuarios_id_usuario",
                table: "Documentos_usuarios",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Historial_estado_maquina_id_estado_calidad",
                table: "Historial_estado_maquina",
                column: "id_estado_calidad");

            migrationBuilder.CreateIndex(
                name: "IX_Historial_estado_maquina_id_maquinaria",
                table: "Historial_estado_maquina",
                column: "id_maquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Incidentes_id_servicio",
                table: "Incidentes",
                column: "id_servicio");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_maquinaria_id_mantenimiento",
                table: "Mantenimientos_maquinaria",
                column: "id_mantenimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_id_estado_calidad",
                table: "Maquinaria",
                column: "id_estado_calidad");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_id_marca_maquinaria",
                table: "Maquinaria",
                column: "id_marca_maquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_id_proveedor",
                table: "Maquinaria",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_id_tipo_maquinaria",
                table: "Maquinaria",
                column: "id_tipo_maquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Marca_maquinaria_id_pais",
                table: "Marca_maquinaria",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "IX_Memorial_id_empleado",
                table: "Memorial",
                column: "id_empleado");

            migrationBuilder.CreateIndex(
                name: "IX_Provedores_id_empresa",
                table: "Provedores",
                column: "id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Provedores_id_producto",
                table: "Provedores",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_id_proveedor",
                table: "Recursos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_id_tipo",
                table: "Recursos",
                column: "id_tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_id_cliente",
                table: "Servicio",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_id_direccion",
                table: "Servicio",
                column: "id_direccion");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_tipo_servicio",
                table: "Servicio",
                column: "tipo_servicio");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_terminado_id_servicio",
                table: "Servicio_terminado",
                column: "id_servicio");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_terminado_satisfaccion",
                table: "Servicio_terminado",
                column: "satisfaccion");

            migrationBuilder.CreateIndex(
                name: "IX_Sub_dominios_id_dominio",
                table: "Sub_dominios",
                column: "id_dominio");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_cliente_id_cliente",
                table: "Telefono_cliente",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_cliente_id_detalle",
                table: "Telefono_cliente",
                column: "id_detalle");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_proveedor_id_detalle",
                table: "Telefono_proveedor",
                column: "id_detalle");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_proveedor_id_proveedor",
                table: "Telefono_proveedor",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_usuarios_id_detalle",
                table: "Telefono_usuarios",
                column: "id_detalle");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_usuarios_id_usuario",
                table: "Telefono_usuarios",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_trabajador_id_direccion",
                table: "Usuario_trabajador",
                column: "id_direccion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_trabajador_id_estado_civil",
                table: "Usuario_trabajador",
                column: "id_estado_civil");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_trabajador_id_genero",
                table: "Usuario_trabajador",
                column: "id_genero");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_trabajador_id_grado_academico",
                table: "Usuario_trabajador",
                column: "id_grado_academico");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_trabajador_id_pais",
                table: "Usuario_trabajador",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_trabajador_id_rol",
                table: "Usuario_trabajador",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_capacitaciones_id_capacitacion",
                table: "Usuarios_capacitaciones",
                column: "id_capacitacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignacion_empleados");

            migrationBuilder.DropTable(
                name: "Asignacion_maquinaria");

            migrationBuilder.DropTable(
                name: "Asignacion_recursos");

            migrationBuilder.DropTable(
                name: "Asignacion_uniformes");

            migrationBuilder.DropTable(
                name: "Carreras_usuario");

            migrationBuilder.DropTable(
                name: "Documentos_usuarios");

            migrationBuilder.DropTable(
                name: "Historial_estado_maquina");

            migrationBuilder.DropTable(
                name: "Incidentes");

            migrationBuilder.DropTable(
                name: "Mantenimientos_maquinaria");

            migrationBuilder.DropTable(
                name: "Memorial");

            migrationBuilder.DropTable(
                name: "Servicio_terminado");

            migrationBuilder.DropTable(
                name: "Telefono_cliente");

            migrationBuilder.DropTable(
                name: "Telefono_proveedor");

            migrationBuilder.DropTable(
                name: "Telefono_usuarios");

            migrationBuilder.DropTable(
                name: "Usuarios_capacitaciones");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Uniformes");

            migrationBuilder.DropTable(
                name: "Mantenimiento");

            migrationBuilder.DropTable(
                name: "Maquinaria");

            migrationBuilder.DropTable(
                name: "Servicio");

            migrationBuilder.DropTable(
                name: "Capacitaciones");

            migrationBuilder.DropTable(
                name: "Usuario_trabajador");

            migrationBuilder.DropTable(
                name: "Estado_calidad");

            migrationBuilder.DropTable(
                name: "Marca_maquinaria");

            migrationBuilder.DropTable(
                name: "Provedores");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Direccion");

            migrationBuilder.DropTable(
                name: "Sub_dominios");

            migrationBuilder.DropTable(
                name: "Dominios");
        }
    }
}
