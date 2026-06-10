using Api_SASL.Models;
using Api_SASL.Modulos.Reportes.DTO;
using Api_SASL.Modulos.Reportes.Interfaz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Api_SASL.Servicios.InterfazServicios;
using System.Text;
using CsvHelper;
using System.Globalization;

namespace Api_SASL.Modulos.Reportes.Endpoints;

public static class EndpointsReportes
{
    public static void MapReportesEndpoints(this IEndpointRouteBuilder app)
    {
        
        var group = app.MapGroup("/Api/Reportes");

//===========================================================================================
        // Agregar incidente
        group.MapPost("/incidentes", async (
            ClaimsPrincipal user, 
            [FromBody] AddIncidente dto, 
            IReportesLogica logica) =>
        {
            var resultado = await logica.agregarIncidenteAsync(user, dto);

            return resultado switch
            {
                Created<Incidente> d => Results.Created($"/Api/Reportes/incidentes/{d.Dato.IdIncidente}", d.Dato),
                ValidationError v => Results.BadRequest(new { error = v.Error}),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .RequireAuthorization("Cliente")
        .WithSummary("Agregar incidente");

// ===============================================================================
        // Listar incidentes (corto)
        group.MapGet("/incidentes", async (IReportesLogica logica) =>
        {
            var lista = await logica.listarIncidenteAsync();
            
            // Como devuelve IEnumerable directamente, si está vacía retornamos 200 OK con array vacío []
            return Results.Ok(lista);
        })
        .RequireAuthorization("PersonalAutorizado")
        .WithSummary("Listar incidentes (corto)");

// ===========================================================================================
        // Mostrar info de incidente detallado
        group.MapGet("/incidentes/{id:int}", async (int id, IReportesLogica logica) =>
        {
            var incidente = await logica.InfoIncidenteAsync(id);
            
            // Tratamos el opcional de C# (nullable) de manera explícita y elegante
            return incidente is not null 
                ? Results.Ok(incidente) 
                : Results.NotFound(new { Mensaje = $"No se encontró el incidente con ID {id}" });
        })
        .RequireAuthorization("PersonalAutorizado")
        .WithSummary("Mostrar info de incidente detallado");

// ===========================================================================================
        // Agregar un memorandum
        group.MapPost("/memorandums", async (
            [FromBody] AddMemorandum dto, 
            IReportesLogica logica) =>
        {
            var resultado = await logica.agregarMemorandumAsync(dto);
            return resultado switch
            {
                Created<Memorial> d => Results.Created($"/Api/Reportes/memorandums/{d.Dato.IdMemorial}/pdf", d.Dato),
                ValidationError v => Results.BadRequest(new { error = v.Error}),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .RequireAuthorization("PersonalAutorizado")
        .WithSummary("Agregar un memorandum");

// ===========================================================================================
        group.MapGet("/memorandums/{id:int}/pdf", async (
            int id, 
            IReportesLogica logica, 
            IWebHostEnvironment env) =>
        {
            
            var resultado = await logica.GenerarMemorandoAsync(id, env);
            return resultado switch
            {
                docCreated m => Results.File(
                    fileContents: m.doc,
                    contentType: "application/pdf",
                    fileDownloadName: $"Memorando_{id}.pdf", 
                    enableRangeProcessing: true
                ),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .RequireAuthorization()
        .WithSummary("Ver memorandum (Generar PDF)");

// ===========================================================================================
        // Reportar estado de maquinaria
        group.MapPost("/estado-maquinaria", async (
            [FromBody] EstadoMaquinaria dto,
            IReportesLogica logica) =>
        {
            var resultado = await logica.reporteEstadoMaquinariaAsync(dto);
            return resultado switch
            {
                Created<HistorialEstadoMaquina> d => Results.Created("/Api/Reportes/estado-maquinaria", d.Dato),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .RequireAuthorization("PersonalAutorizado")
        .WithSummary("Reportar estado de maquinaria");

// ===========================================================================================
        // Listar historial de estado de maquinaria
        group.MapGet("/estado-maquinaria", async (IReportesLogica logica) =>
        {
            var lista = await logica.ListHistorialsAsync();
            return Results.Ok(lista);
        })
        .RequireAuthorization("PersonalAutorizado")
        .WithSummary("Listar historial de estado de maquinaria");

// ===========================================================================================
        // descargar csv historial de estado de maquinaria
        group.MapGet("/exportar-csv", async (IReportesLogica logica) =>
        {
            var hostorial = await logica.ListHistorialsAsync();

            if (!hostorial.Any())
            {
                return Results.NotFound(new { mensaje = "No hay datos disponibles para exportar." });
            }

            // Flujo en memoria RAM para armar el archivo sobre la marcha
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Agregamos la instrucción especial para que Excel no rompa las columnas
                await writer.WriteLineAsync("sep=,");
                
                await csv.WriteRecordsAsync(hostorial);
                await writer.FlushAsync();
            }

            // Rebobinamos el puntero al inicio
            memoryStream.Position = 0;

            return Results.File(
                fileStream: memoryStream,
                contentType: "text/csv",
                fileDownloadName: $"Reporte_hostorial_{DateTime.Now:yyyyMMdd}.csv"
            );
        })
        .WithSummary("Genera y descarga en tiempo real un archivo CSV con la información completa de el historial de estado de maquinaria.")
        .RequireAuthorization("PersonalAutorizado");


    }
}