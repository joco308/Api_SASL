using Api_SASL.Modulos.Servicios.DTO;
using Api_SASL.Modulos.Servicios.Interfaz;
using Api_SASL.Modulos.Servicios.Logica;
using Api_SASL.Servicios.InterfazServicios;
using Microsoft.Identity.Client;
using System.Text;
using CsvHelper;
using System.Globalization;

namespace Api_SASL.Modulos.Servicios.Endpoints;
public static class ServiciosEndpoints
{
    public static void MapServiciosEndpoints(this IEndpointRouteBuilder app)
    {        
        // Definición del grupo de servicios
        var group = app.MapGroup("/Api/Servicios");


//=======================================================================================================
        // Añadir un servicio nuevo
        group.MapPost("/Nuevo", async (AñadirServicio dto, IServiciosLogica modulo) =>
        {
            var resultado = await modulo.añadirServicioNuevoAsync(dto);

            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Servicio y asignaciones creados correctamente" }),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Añade un servicio necesita parametros AñadirServicio")
        .RequireAuthorization("PersonalAutorizado");


//=======================================================================================================
        // Mostrar todos los servicios (Lista resumida)
        group.MapGet("/", async (IServiciosLogica modulo) =>
        {
            var lista = await modulo.mostrarServiciosAsync();
            return Results.Ok(lista);
        })
        .WithSummary("Muestra los servicios en forma de lista resumida")
        .RequireAuthorization("PersonalAutorizado");


//=======================================================================================================
        // Mostrar información detallada de un servicio específico
        group.MapGet("/{id:int}", async (int id, IServiciosLogica modulo) =>
        {
            var servicio = await modulo.informacionServicioAsync(id);
            
            return servicio is not null 
                ? Results.Ok(servicio) 
                : Results.NotFound(new { mensaje = $"No se encontró el servicio con ID {id}" });
        })
        .WithSummary("Muestra los detalles de un servicio por su id")
        .RequireAuthorization("PersonalAutorizado");


//=======================================================================================================
        // Asignar empleado a un servicio
        group.MapPost("/Asignar-empleado", async (AsignarUsuariosServicios dto, IServiciosLogica modulo) =>
        {
            var resultado = await modulo.asignarEmpleadoServicioAsync(dto);

            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Empleado asignado al servicio exitosamente" }),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Añade un usuarioTrabajador a un servicio por sus id")
        .RequireAuthorization("PersonalAutorizado");


//=======================================================================================================
        // Asignar Maquinaria a un servicio
        group.MapPost("/asignar-maquinaria", async (AsignarMaquinariaServicios dto, IServiciosLogica modulo) =>
        {
            var resultado = await modulo.asignarMaquinariaServicioAsync(dto);

            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Maquinaria asignada correctamente" }),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Añade una maquinaria a un servicio por sus id")
        .RequireAuthorization("PersonalAutorizado");


//=======================================================================================================
        // Asignar Recurso a un servicio
        group.MapPost("/asignar-recurso", async (AsignarRecursoServicios dto, IServiciosLogica modulo) =>
        {
            var resultado = await modulo.asignarRecursoServicioAsync(dto);

            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Recurso asignado correctamente" }),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Añade un recuro a un servicio por sus id")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Descargar CSV de servicios
        group.MapGet("/exportar-csv", async (IServiciosLogica logica) =>
        {
            var servicios = await logica.datosServicioParaCSVAsync();

            if (!servicios.Any())
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
                
                await csv.WriteRecordsAsync(servicios);
                await writer.FlushAsync();
            }

            // Rebobinamos el puntero al inicio
            memoryStream.Position = 0;

            return Results.File(
                fileStream: memoryStream,
                contentType: "text/csv",
                fileDownloadName: $"Reporte_Servicios_{DateTime.Now:yyyyMMdd}.csv"
            );
        })
        .WithSummary("Genera y descarga en tiempo real un archivo CSV con la información completa de todos los servivios.")
        .RequireAuthorization("PersonalAutorizado");



    }
}
