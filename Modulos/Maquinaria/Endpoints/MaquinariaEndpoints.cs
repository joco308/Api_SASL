using Api_SASL.Modulos.Maquinaria.DTO;
using Api_SASL.Modulos.Maquinaria.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;

namespace Api_SASL.Modulos.Maquinaria.Endpoints;

public static class MaquinariaEndpoints
{
    public static void MapMaquinariaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Api/Maquinaria");

//=======================================================================================================
        // Listar todas las maquinarias (versión resumida)
        group.MapGet("/", async (IMaquinariaLogica logica) =>
        {
            var maquinas = await logica.listarMaquinariasAsync();
            return Results.Ok(maquinas);
        })
        .WithSummary("Listar todas las maquinarias (versión resumida)")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Obtener información detallada de una maquinaria por ID
        group.MapGet("/{id:int}", async (int id, IMaquinariaLogica logica) =>
        {
            var maquinaria = await logica.informacionMaquinarioAsync(id);
            
            return maquinaria is null 
                ? Results.NotFound(new { mensaje = $"No se encontró maquinaria con ID {id}" }) 
                : Results.Ok(maquinaria);
        })
        .WithSummary("Obtener información detallada de una maquinaria por ID")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Añadir una nueva maquinaria
        group.MapPost("/", async (AgregarMaquinaria dto, IMaquinariaLogica logica) =>
        {
            var resultado = await logica.añadirMaquinariaAsync(dto);
            return resultado switch
            {
                Created<Maquinarium> m => Results.Created($"/Api/Maquinaria/mantenimiento/{m.Dato.IdMaquinaria}", m.Dato),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Añadir una nueva maquinaria")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Listar todas las marcas de maquinaria
        group.MapGet("/marcas", async (IMaquinariaLogica logica) =>
        {
            var marcas = await logica.listarMarcasMAquinariaAsync();
            return Results.Ok(marcas);
        })
        .WithSummary("Listar todas las marcas de maquinaria")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Añadir una nueva marca
        group.MapPost("/marcas", async (AgragarMarcaMaquinaria dto, IMaquinariaLogica logica) =>
        {
            var resultado = await logica.añadirMarcaMaquinariaAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Añadir una nueva marca")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Listar estados de calidad (Subdominios)
        group.MapGet("/estados", async (IMaquinariaLogica logica) =>
        {
            var estados = await logica.listarEstadosMacAsync();
            return Results.Ok(estados);
        })
        .WithSummary("Listar estados de calidad (Subdominios)")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Mostra info de una maquinaria (vercion resumida)
        group.MapGet("/Short{id:int}", async (int id, IMaquinariaLogica logica) =>
        {
            var maquinaria = await logica.mostrarInfoResumidaMaquinaria(id);
            
            return maquinaria is null 
                ? Results.NotFound(new { mensaje = $"No se encontró maquinaria con ID {id}" }) 
                : Results.Ok(maquinaria);
        })
        .WithSummary("Mostra info de una maquinaria (vercion resumida)");

//=======================================================================================================
        // Añadir mantenimiento a una maquinaria
        group.MapPost("/mantenimiento", async (AddManteniminetoMaquinaria dto, IMaquinariaLogica logica) =>
        {
            var resultado = await logica.manteniminetoMaquinariaAsync(dto);
            return resultado switch
            {
                Created<Mantenimiento> m => Results.Created($"/Api/Maquinaria/mantenimiento/{m.Dato.IdMantenimiento}", m.Dato),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Añadir un mantenimiento y asignarlo a una maquinaria")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Listar mantenimientos
        group.MapGet("/mantenimiento", async (IMaquinariaLogica logica) =>
        {
            var lista = await logica.ListarMantenimintosAsync();
            return Results.Ok(lista);
        })
        .WithSummary("Muestra la lista de mantenimientos")
        .RequireAuthorization("PersonalAutorizado");

//=======================================================================================================
        // Mostrar información detallada de un mantenimiento
        group.MapGet("/mantenimiento/{id:int}", async (int id, IMaquinariaLogica logica) =>
        {
            var mantenimiento = await logica.mostrarInfoMantenimintoAsync(id);
            
            return mantenimiento is null 
                ? Results.NotFound(new { mensaje = $"No se encontró el mantenimiento con ID {id}" }) 
                : Results.Ok(mantenimiento);
        })
        .WithSummary("Muestra los detalles de un mantenimiento por su id")
        .RequireAuthorization("PersonalAutorizado");

    }




    private static IResult ManejarResultado(IResultadoServicio resultado)
    {
        return resultado switch
        {
            Success => Results.Ok(new { mensaje = "Operación realizada con éxito" }),
            ValidationError v => Results.BadRequest(new { error = v.Error }),
            NotFound n => Results.NotFound(new { error = n.Mensaje }),
            _ => Results.StatusCode(500)
        };
    }
}
