using Api_SASL.Modulos.Servicios.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Servicios.Interfaz;

public interface IServiciosLogica
{

    // Añadir un servicio
    Task<IResultadoServicio> añadirServicioNuevoAsync(AñadirServicio ad);

    // Mostrar Servicios
    Task<IEnumerable<ListarServicio>> mostrarServiciosAsync();

    //Mostrar informacion de un servicio
    Task<InfoServicio?> informacionServicioAsync(int idServicio);

    // asignar empleado con servicio
    Task<IResultadoServicio> asignarEmpleadoServicioAsync(AsignarUsuariosServicios entrada);

    // asignar Maquinaria con servicio
    Task<IResultadoServicio> asignarMaquinariaServicioAsync(AsignarMaquinariaServicios entrada);

    // asignar recurso con servicio
    Task<IResultadoServicio> asignarRecursoServicioAsync(AsignarRecursoServicios entrada);
}