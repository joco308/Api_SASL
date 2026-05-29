using Api_SASL.Modulos.Clientes.DTO;
using Api_SASL.Modulos.Clientes.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Microsoft.AspNetCore.Mvc;
using Api_SASL.Models;

namespace Api_SASL.Modulos.Clientes.Endpoints;

public static class ClientesEndpoints
{
    public static void MapClientesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Api/Clientes");

    // ========================================================================
        // iniciar secion y mandar 2FA
        group.MapPost("/solicitar-2fa", async (ClienteLogin dto, IClientesLogica modulo) =>
            {
                var resultado = await modulo.manfar2FAAsync(dto);

                return resultado switch
                {
                    Success => Results.Ok(new { mensaje = "Código enviado al correo" }),
                    NotFound n => Results.Json(new { mensaje = "Credenciales incorrectas." }, statusCode: 401),
                    _ => Results.StatusCode(500)
                };
            })
            .WithSummary("Pide correo y contraseña y manda al correo 2FA");

    // ========================================================================
        // verificar el 2FA
        group.MapPost("/verificar-2fa", async (Cliente2Fa dto, IClientesLogica modulo, HttpContext context) =>
            {
                var resultado = await modulo.verificarCodigo2FAAsyncMandarTokenAsync(dto);

                return resultado switch
                {
                    SuccessWithToken s => CrearCookieSesion(context, s.Token),
                    ValidationError v => Results.BadRequest(new { error = v.Error }),
                    NotFound n => Results.NotFound(new { error = n.Mensaje }),
                    _ => Results.StatusCode(500)
                };
            })
            .WithSummary("Pide el codifo 2FA y entrega en token en cookes");


    // ========================================================================
        // listar clientes
        group.MapGet("/", async (IClientesLogica modulo) =>
        {
            var clientes = await modulo.listarClientesCortoAsync();
            return Results.Ok(clientes);
        })
        .WithSummary("Obtiene la lista resumida de todos los clientes.")
        .RequireAuthorization("PersonalAutorizado");


    // ========================================================================
        // mostrar info de un cliente
        group.MapGet("/{id:int}", async ([FromRoute] int id, IClientesLogica modulo) =>
        {
            var cliente = await modulo.mostrarInfoClienteAsync(id);

            return cliente is null
                ? Results.NotFound(new { mensaje = $"No se encontró el cliente con ID {id}" })
                : Results.Ok(cliente);
        })
        .WithSummary("Retorna la información detallada de un cliente.")
        .RequireAuthorization("PersonalAutorizado");


    // ========================================================================
        // añadir cliente
        group.MapPost("/", async (AñadirCliente dto, IClientesLogica modulo) =>
        {
            var resultado = await modulo.añadirClienteAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Registra un nuevo cliente.");
    }



    private static IResult ManejarResultado(IResultadoServicio resultado)
    {
        return resultado switch
        {
            Created<Cliente> c => Results.Created($"/Api/Clientes/{c.Dato.IdCliente}", c.Dato),
            ValidationError v => Results.BadRequest(new { error = v.Error }),
            NotFound n => Results.NotFound(new { error = n.Mensaje }),
            _ => Results.StatusCode(500)
        };
    }

    private static IResult CrearCookieSesion(HttpContext context, string token)
    {
        context.Response.Cookies.Append("token_sesion", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(8)
        });

        return Results.Ok(new { mensaje = "Autenticación exitosa" });
    }
}
