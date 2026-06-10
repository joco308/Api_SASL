using System.Security.Claims;
using Api_SASL.Modulos.Cobros.DTO;
using Api_SASL.Modulos.Cobros.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_SASL.Modulos.Cobros.Endpoints;

public static class CobrosEndpoints
{
    public static void MapCobrosEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Api/Cobros");

        // ====================================================================
        // Listar todos los cobros
        group.MapGet("/", async (ICobrosLogica logica) =>
        {
            var cobros = await logica.listarCobrosAsync();
            return Results.Ok(cobros);
        })
        .WithSummary("Lista resumida de todos los cobros.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Información detallada de un cobro
        group.MapGet("/{id:int}", async ([FromRoute] int id, ICobrosLogica logica) =>
        {
            var cobro = await logica.infoCobroAsync(id);
            return cobro is not null
                ? Results.Ok(cobro)
                : Results.NotFound(new { mensaje = $"No se encontró el cobro con ID {id}" });
        })
        .WithSummary("Detalles de un cobro por su ID.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Información detallada de un cobro de usuario autenticado
        group.MapGet("/mis-cobros/{id:int}", async ([FromRoute] int id, ICobrosLogica logica, ClaimsPrincipal user) =>
        {
            var cobro = await logica.infoCobroClienteAsync(user, id);
            return cobro is not null
                ? Results.Ok(cobro)
                : Results.NotFound(new { mensaje = $"No se encontró el cobro con ID {id}" });
        })
        .WithSummary("Detalles de un cobro por su ID.")
        .RequireAuthorization("Cliente");

        // ====================================================================
        // Listar cobros del cliente autenticado
        group.MapGet("/mis-cobros", async (ClaimsPrincipal user, ICobrosLogica logica) =>
        {
            var cobros = await logica.listarCobrosPorClienteAsync(user);
            return cobros is not null
                ? Results.Ok(cobros)
                : Results.NotFound(new { mensaje = "No se encontraron cobros para este cliente." });
        })
        .WithSummary("Lista los cobros del cliente autenticado.")
        .RequireAuthorization("Cliente");

        // ====================================================================
        // Crear un nuevo cobro
        group.MapPost("/", async (CrearCobro dto, ICobrosLogica logica) =>
        {
            var resultado = await logica.crearCobroAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Registra un nuevo cobro.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Registrar un pago a un cobro
        group.MapPost("/pagos", async (RegistrarPago dto, ICobrosLogica logica) =>
        {
            var resultado = await logica.registrarPagoAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Registra un cobro asociado a un pago.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Listar pagos de un cobro
        group.MapGet("/{idCobro:int}/pagos", async ([FromRoute] int idCobro, ICobrosLogica logica) =>
        {
            var pagos = await logica.listarPagosPorCobroAsync(idCobro);
            return Results.Ok(pagos);
        })
        .WithSummary("Lista los pagos realizados para un cobro específico.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Notificar pago realizado (el cliente avisa que pagó)
        group.MapPost("/{idCobro:int}/notificar-pago", async ([FromRoute] int idCobro, ClaimsPrincipal user, ICobrosLogica logica) =>
        {
            var resultado = await logica.notificarPagoRealizadoAsync(idCobro, user);
            return ManejarResultado(resultado);
        })
        .WithSummary("El cliente notifica que realizó un pago.")
        .RequireAuthorization("Cliente");

        // ====================================================================
        // Subir QR (multipart)
        group.MapPost("/qrs", async (ClaimsPrincipal user, [FromForm] CrearQr dto, IWebHostEnvironment env, ICobrosLogica logica) =>
        {
            var resultado = await logica.crearQrAsync(user, dto, env);
            return ManejarResultado(resultado);
        })
        .WithSummary("Sube un código QR al servidor.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Listar todos los QRs
        group.MapGet("/qrs", async (ICobrosLogica logica) =>
        {
            var qrs = await logica.listarQrsAsync();
            return Results.Ok(qrs);
        })
        .WithSummary("Lista todos los códigos QR registrados.")
        .RequireAuthorization("PersonalAutorizado");

        // ====================================================================
        // Descargar imagen de un QR
        group.MapGet("/qrs/{idQr:int}/imagen", async ([FromRoute] int idQr, IWebHostEnvironment env, ICobrosLogica logica) =>
        {
            var resultado = await logica.mandarQrAsync(idQr, env);
            if (resultado is null)
                return Results.NotFound(new { mensaje = $"No se encontró el QR con ID {idQr}" });

            return Results.File(
                fileStream: resultado.qr,
                contentType: "image/png"
            );
        })
        .WithSummary("Descarga la imagen de un código QR.")
        .RequireAuthorization();
    }

    private static IResult ManejarResultado(IResultadoServicio resultado)
    {
        return resultado switch
        {
            Created<Cobro> c => Results.Created($"/Api/Cobros/{c.Dato.IdCobro}", c.Dato),
            Created<Pago> p => Results.Created($"/Api/Cobros/{p.Dato.IdCobro}/pagos", p.Dato),
            Created<Qr> q => Results.Created($"/Api/Cobros/qrs/{q.Dato.IdQr}/imagen", q.Dato),
            Success => Results.Ok(new { mensaje = "Operación realizada con éxito" }),
            ValidationError v => Results.BadRequest(new { error = v.Error }),
            NotFound n => Results.NotFound(new { error = n.Mensaje }),
            _ => Results.StatusCode(500)
        };
    }
}
