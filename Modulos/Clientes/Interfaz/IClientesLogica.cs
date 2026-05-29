using Api_SASL.Modulos.Clientes.DTO;
using Api_SASL.Modulos.Reportes.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Clientes.Interfaz;

public interface IClientesLogica
{
    // iniciar secion clientes
    Task<IResultadoServicio> manfar2FAAsync(ClienteLogin us);

    // mandar doble factor de autenticacion
    Task<IResultadoServicio> verificarCodigo2FAAsyncMandarTokenAsync(Cliente2Fa login);

    // Mostrar informacion de un Cliente
    Task<InfoCleinte?> mostrarInfoClienteAsync(int idCliente);

    // listar clientes
    Task<IEnumerable<InfoClienteCorto>> listarClientesCortoAsync();

    // añadir cliente
    Task<IResultadoServicio> añadirClienteAsync(AñadirCliente n);
}