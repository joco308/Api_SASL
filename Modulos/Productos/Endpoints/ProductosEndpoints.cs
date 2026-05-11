using Api_SASL.Modulos.Productos.DTO;
using Api_SASL.Modulos.Productos.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Microsoft.AspNetCore.Mvc;

namespace Api_SASL.Modulos.Productos.Endpoints;

public static class ProductosEndpoints
{
    public static void MapProductosEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Api/Productos");
            

//====================================================================================================
        // Listar todos los recursos/productos
        group.MapGet("/", async (IProductosLogica logica) =>
        {
            var productos = await logica.listarRecursos();
            return Results.Ok(productos);
        })
        .WithSummary("Listar todos los recursos/productos");

//====================================================================================================
        // Añadir un nuevo recurso
        group.MapPost("/", async (AñadirRecurso dto, IProductosLogica logica) =>
        {
            var resultado = await logica.añadirRecursoAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Añadir un nuevo recurso")
        .RequireAuthorization("PersonalAutorizado");

//====================================================================================================
        // Editar solo el nombre del producto
        group.MapPatch("/editar/nombre", async (EditarNombre ent, IProductosLogica logica) =>
        {
            var resultado = await logica.editarNombreProductoAsync(ent);
            return ManejarResultado(resultado);
        })
        .WithSummary("Editar solo el nombre del producto")
        .RequireAuthorization("PersonalAutorizado");

//====================================================================================================
        // Editar solo la descripción del producto
        group.MapPatch("/editar/descripcion", async (EditarDescripcion ent, IProductosLogica logica) =>
        {
            var resultado = await logica.editarDescripcionAsync(ent);
            return ManejarResultado(resultado);
        })
        .WithSummary("Editar solo la descripción del producto")
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