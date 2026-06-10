using Api_SASL.Modulos.Maquinaria.DTO;
using Api_SASL.Modulos.Reportes.DTO;
using Api_SASL.Servicios;
using Api_SASL.Servicios.InterfazServicios;
using System.Security.Claims;

namespace Api_SASL.Modulos.Reportes.Interfaz;

public interface IReportesLogica
{
    // Agregar incidente
    Task<IResultadoServicio> agregarIncidenteAsync(ClaimsPrincipal user, AddIncidente incidente);

    // listar incidentes (corto)
    Task<IEnumerable<ListaIncidente>> listarIncidenteAsync();

    // Mostra info de incidente detallado
    Task<infoIncidente?> InfoIncidenteAsync(int idIncidente);

    // Agregar un memorandum
    Task<IResultadoServicio> agregarMemorandumAsync(AddMemorandum memo);

    // Ver memoranum 
    Task<IResultadoServicio> GenerarMemorandoAsync(int idMemo, IWebHostEnvironment env);

    // Reporte de estado maquinaria
    Task<IResultadoServicio> reporteEstadoMaquinariaAsync(EstadoMaquinaria ent);

    // listar reportes de hitotrial de estado de maquinaria
    Task<IEnumerable<ListHistorialEstadoMaquinaria>> ListHistorialsAsync();


}