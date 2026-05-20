using Api_SASL.Modulos.Trabajadores.Interfaz;
using Api_SASL.Modulos.Trabajadores.DTO;
using Api_SASL.Modulos.Trabajadores.Logica;
using Api_SASL.Servicios.InterfazServicios;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using CsvHelper;
using System.Globalization;

namespace Api_SASL.Modulos.Trabajadores.Endpoints;

public static class TrabajadoresEndpoints
{
    public static void MapTrabajadoresEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/Api/Trabajadores");


// ===========================================================================================
        // Mostrar informacion de 1 usuario por el ci
        group.MapGet("/{ci:int}", async ([FromRoute] int ci, ITrabajadoresLogica modulo) =>
        {
            var datos = await modulo.VerInfoUsuarioAsync(ci);

            return datos is not null
                ? Results.Ok(datos)
                : Results.NotFound(new { mensaje = "Algo salio mal"});
        })
        .WithSummary("Se optine informacion de 1 usuario por el ci")
        .RequireAuthorization("PersonalAutorizado");

// ===========================================================================================
        // listar roles
        group.MapGet("/roles", async (ITrabajadoresLogica modulo) =>
        {
            var datos = await modulo.ListarRolesAsync();

            return datos.Any()
                ? Results.Ok(datos)
                : Results.NotFound(new { mensaje = "Algo salio mal"});
        })
        .WithSummary("listar roles")
        .RequireAuthorization("PersonalAutorizado");

// ===========================================================================================
        // Añadir telefonos a trabajadores
        group.MapPost("/telefonos", async (AñadirTelefonoTrabajadores dto, ITrabajadoresLogica logica) =>
        {
            var resultado = await logica.añadirTelefonosAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Asocia una lista de números telefónicos a un trabajador específico.")
        .RequireAuthorization("PersonalAutorizado");

// ===========================================================================================
        // Exportar Información Completa a CSV
        group.MapGet("/exportar-csv", async (ITrabajadoresLogica logica) =>
        {
            var usuarios = await logica.InfoUsuarioCSVAsync();

            if (!usuarios.Any())
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
                
                await csv.WriteRecordsAsync(usuarios);
                await writer.FlushAsync();
            }

            // Rebobinamos el puntero al inicio
            memoryStream.Position = 0;

            return Results.File(
                fileStream: memoryStream,
                contentType: "text/csv",
                fileDownloadName: $"Reporte_Trabajadores_{DateTime.Now:yyyyMMdd}.csv"
            );
        })
        .WithSummary("Genera y descarga en tiempo real un archivo CSV con la información completa de todos los trabajadores.")
        .RequireAuthorization("PersonalAutorizado");



    }


    private static IResult ManejarResultado(IResultadoServicio resultado)
    {
        return resultado switch
        {
            Success => Results.Ok(new { mensaje = "Operación realizada con éxito." }),
            ValidationError v => Results.BadRequest(new { error = v.Error }),
            NotFound n => Results.NotFound(new { error = n.Mensaje }),
            SuccessWithToken t => Results.Ok(new { token = t.Token }),
            _ => Results.StatusCode(500)
        };
    }
}