using Api_SASL.Modulos.Provedores.DTO;
using Api_SASL.Modulos.Provedores.Interfaz;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Provedores.Endpoints;

public static class ProveedoresEndpoints
{
    public static void MapProveedoresEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Api/Proveedores");

// ========================================================================================
        // Listar Proveedores
        group.MapGet("/", async (IProvedoresLogica logica) =>
        {
            var proveedores = await logica.ListarProvedoresAsync();
            return Results.Ok(proveedores);
        })
        .WithSummary("Obtiene una lista resumida de todos los proveedores registrados con sus teléfonos.")
        .RequireAuthorization("PersonalAutorizado");

// ========================================================================================
        // Información detallada de un Proveedor
        group.MapGet("/{id:int}", async (int id, IProvedoresLogica logica) =>
        {
            var proveedor = await logica.InformacionProvedorAsync(id);
            
            return proveedor is null 
                ? Results.NotFound(new { mensaje = $"No se encontró el proveedor con ID {id}" }) 
                : Results.Ok(proveedor);
        })
        .WithSummary("Retorna la información detallada de un proveedor, incluyendo su NIT y lista de productos.")
        .RequireAuthorization("PersonalAutorizado");

// ========================================================================================
        // Añadir un Proveedor
        group.MapPost("/", async (AñadirProvedor dto, IProvedoresLogica logica) =>
        {
            var resultado = await logica.añadirProvedorAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Registra un nuevo proveedor asociado a una empresa existente.")
        .RequireAuthorization("PersonalAutorizado");

// ========================================================================================
        // Agregar Teléfono a un Proveedor
        group.MapPost("/telefono", async (AgregarTelefonoProvedor dto, IProvedoresLogica logica) =>
        {
            var resultado = await logica.agregarTelefonoProvedor(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Asocia un nuevo número telefónico y su detalle a un proveedor específico.")
        .RequireAuthorization("PersonalAutorizado");

// ========================================================================================
        // Editar Nombre del Proveedor
        group.MapPatch("/nombre", async (IdmasNombre dto, IProvedoresLogica logica) =>
        {
            var resultado = await logica.editarNombreAsync(dto);
            return ManejarResultado(resultado);
        })
        .WithSummary("Actualiza el nombre de contacto de un proveedor mediante su ID.")
        .RequireAuthorization("PersonalAutorizado");
    }

    private static IResult ManejarResultado(IResultadoServicio resultado)
    {
        return resultado switch
        {
            Success => Results.Ok(new { mensaje = "Operación realizada con éxito" }),
            ValidationError v => Results.BadRequest(new { error = v.Error }),
            NotFound n => Results.NotFound(new { error = n.Mensaje }),
            SuccessWithToken t => Results.Ok(new { token = t.Token }),
            _ => Results.StatusCode(500)
        };
    }
}