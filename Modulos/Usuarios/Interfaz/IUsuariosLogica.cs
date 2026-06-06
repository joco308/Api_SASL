using Api_SASL.Modulos.Usuarios.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Usuarios.Interfaz;

public interface IUsuariosLogica
{
    // Mandar 2FA y verificar usuario y contraseña
    Task<IResultadoServicio> mandar2FA(UsuarioLogin us);

    // Verificar el codigo de 2FA
    Task<IResultadoServicio> verificarCodigo2FAAsyncMandarToken(Login2FA login);

    // Añadir usuarios
    Task<IResultadoServicio> añadirUsuarioAsync(NuevoUsiario nv);

    // Editar usuario direccion
    Task<IResultadoServicio> editarUsuarioDireccion(EditarDireccion ed);

    // Editar usuario el rol
    Task<IResultadoServicio> editarUsuarioRol(EditarRol ed);

    // mostrar usuarios
    Task<IEnumerable<UsuarioDatos>> usuarios();

    // mostrar usuarios por servicio
    Task<IEnumerable<UsuarioDatos>> UsuariosFiltados(bool servicio);

    // subir documentos de los usuarios
    Task<IResultadoServicio> subirArchivoUsuarioAsync(IFormFile archivo, IWebHostEnvironment env, DatosParaSubirDoc doc);

    // Mostrar un archivo por id 
    Task<IResultadoServicio> mandarRutaDeArchivoAsync(int ent);

    // Añadir carrear universitaria
    Task<IResultadoServicio> añadoirCarreaUniversitariaUsuarioAsync(AñadirCarrera ent);

    // Listar docuemntos de un usuario por tipo
    Task<IEnumerable<DocumentosUsuarioTipo>> listDocuemntosUsuarioTipoAsync(PedirDocumentos ent);

    // Agregar una capasitacion
    Task<IResultadoServicio> agregarCapasitacionAsync(AñadirCapasitacion ent);

    // Poner usuario a capasitacion
    Task<IResultadoServicio> usuarioCapasitacionAsync(PonerUsuarioCapasitacion ent);

    // Listar capasitaciones
    Task<IEnumerable<ListarCapasitaciones>> listarCapasitacionesAsync();

    // Mostrar info de 1 Capasitacion
    Task<InfoCapasitacion?> InfoCapasitacionAsync(int IdCapacitacion);

    // Añadir uniformes
    Task<IResultadoServicio> añadirUniformeAsync(AñadirUniforme ent);

    // Listar uniformes
    Task<IEnumerable<ListarUniformes>> ListarUniformesAsync();

    // Asginar uniforme a empleado
    Task<IResultadoServicio> asignarUniformeAsync(AsignarUniformeEmpleado ent);

    // Listar empleados con uniforme
    Task<IEnumerable<UsuariosUniformes>> listarEmpleadosUniformeAsync();

    









    // obtener catalogo de los dominios
    Task<IEnumerable<CatalogoDTO>> ObtenerCatalogoPorDominioAsync(string nombreDominio);

}