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

    // ver memoranum 
    Task<IResultadoServicio> GenerarMemorandoAsync(int idMemo, IWebHostEnvironment env);



}