using System.Security.Claims;
using Api_SASL.Modulos.Cobros.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Cobros.Interfaz;

public interface ICobrosLogica
{
    // Crear Cobros
    Task<IResultadoServicio> crearCobroAsync(CrearCobro dto);

    // Listar todos los cobors cobros
    Task<IEnumerable<ListarCobro>> listarCobrosAsync();

    // Info Cobros
    Task<InfoCobro?> infoCobroAsync(int idCobro);

    // Registrar pago
    Task<IResultadoServicio> registrarPagoAsync(RegistrarPago dto);

    // Listar pagos por cobro
    Task<IEnumerable<ListarPago>> listarPagosPorCobroAsync(int idCobro);

    // Crear qr
    Task<IResultadoServicio> crearQrAsync(ClaimsPrincipal user, CrearQr ent, IWebHostEnvironment env);

    // mandar qr
    Task<MandarQr?> mandarQrAsync(int IdQr, IWebHostEnvironment env);

    // listar qrs
    Task<IEnumerable<ListarQr>> listarQrsAsync();

    // Listar cobors de 1 solo cliente
    Task<IEnumerable<ListarCobro>?> listarCobrosPorClienteAsync(ClaimsPrincipal user); 

    // Notificar pago realizado
    Task<IResultadoServicio> notificarPagoRealizadoAsync(int IdCobro, ClaimsPrincipal user);

    // Info cobro para cliente
    Task<InfoCobroCliente?> infoCobroClienteAsync(ClaimsPrincipal user, int IdCobro);

}
