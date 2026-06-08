using Api_SASL.Modulos.Servicios.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Servicios.Interfaz;

public interface IServiciosLogica
{

    // Añadir un servicio
    Task<IResultadoServicio> añadirServicioNuevoAsync(AñadirServicio ad);

    // Mostrar Servicios
    Task<IEnumerable<ListarServicio>> mostrarServiciosAsync();

    // Mostrar informacion de un servicio
    Task<InfoServicio?> informacionServicioAsync(int idServicio);

    // Asignar empleado con servicio
    Task<IResultadoServicio> asignarEmpleadoServicioAsync(AsignarUsuariosServicios entrada);

    // Asignar Maquinaria con servicio
    Task<IResultadoServicio> asignarMaquinariaServicioAsync(AsignarMaquinariaServicios entrada);

    // Asignar recurso con servicio
    Task<IResultadoServicio> asignarRecursoServicioAsync(AsignarRecursoServicios entrada);

    // Mostrar horarios
    Task<IEnumerable<HorarioDTO>> mostrarHorariosAsync();

    // Descargar archivo csv
    Task<IEnumerable<InfoServicio>> datosServicioParaCSVAsync();

    // Servicio terminado
    Task<IResultadoServicio> servicioTerminadoAsync(AddServicioTerminado ent);

    // Listar servicios temrinados
    Task<IEnumerable<ListarServicioTerminado>> listarServicioTerminadosAsync();

    // Mostrar info de Servicio terminado
    Task<InfoServicioTerminado?> infoServicioTerminadoAsync(int idServicio);
}
