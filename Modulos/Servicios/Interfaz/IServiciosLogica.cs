using Api_SASL.Modulos.Servicios.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Servicios.Interfaz;

public interface IServiciosLogica
{

    // Añadir un servicio
    Task<IResultadoServicio> añadirServicioNuevoAsync(AñadirServicio ad);

    // Mostrar Servicios
    Task<IEnumerable<ListarServicio>> mostrarServiciosAsync();
}