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
    Task<IResultadoServicio> mandarRutaDeArchivoAsync(PedirDocumento ent);

    Task<IResultadoServicio> añadoirCarreaUniversitariaUsuarioAsync(AñadirCarrera ent);




    // obtener catalogo de los dominios
    Task<IEnumerable<CatalogoDTO>> ObtenerCatalogoPorDominioAsync(string nombreDominio);

}