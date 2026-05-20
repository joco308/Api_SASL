using Api_SASL.Modulos.Trabajadores.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Trabajadores.Interfaz;

public interface ITrabajadoresLogica
{
    // listar Roles
    Task<IEnumerable<ListarRoles>> ListarRolesAsync();

    // Añadir telefonos a trabajadores
    Task<IResultadoServicio> añadirTelefonosAsync(AñadirTelefonoTrabajadores ent);

    // Mostrar informacion de un usuario
    Task<VerInfoUsuarioId?> VerInfoUsuarioAsync(int ci);

    //pasar informacion completa de usuairos en csv
    Task<IEnumerable<VerInfoUsuario>> InfoUsuarioCSVAsync();
}