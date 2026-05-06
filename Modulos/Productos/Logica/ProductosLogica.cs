using Api_SASL.Modulos.Productos.DTO;
using Api_SASL.Modulos.Productos.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_SASL.Modulos.Productos.Logica;

public class ProductosLogica : IProductosLogica
{
    public readonly DevSaslContext _db;

    ProductosLogica(DevSaslContext db)
    {
        _db = db;
    }

// -------------------------------------------------------------------------------
    // Añadir Recursos
    public async Task<IResultadoServicio> añadirRecursoAsync(AñadirRecurso ent)
    {
        var proveor = await _db.Provedores.FindAsync(ent.IdProvedor);
        var tipo = await _db.SubDominios.FindAsync(ent.IdTipo);

        if(proveor is null || tipo is null) return new NotFound("No se se encontro el provedor o el tipo");

        var recurso = new Recurso
        {
            IdProveedorNavigation = proveor,
            IdTipoNavigation = tipo,
            Nombre = ent.nombre,
            Descripcion = ent.Descripcion
        };

        _db.Recursos.Add(recurso);

        return await guardarCambiosAsync();
    }

// -------------------------------------------------------------------------------
    // Listar Recursos
    public async Task<IEnumerable<ListarRecurso>> listarRecursos()
    {
        return await _db.Recursos
            .Select(
                u => new ListarRecurso(
                    u.IdProveedorNavigation.Nombre,
                    u.IdProveedorNavigation.IdEmpresaNavigation.Detalle,
                    u.IdTipoNavigation.Detalle,
                    u.Nombre,
                    u.Descripcion
                )
            ).ToListAsync();
    }

// -------------------------------------------------------------------------------
    // Editar Nombre Recurso
    public async Task<IResultadoServicio> editarNombreProductoAsync(EditarNombre ent)
    {
        var recurso = await _db.Recursos.FindAsync(ent.IdRecurso);

        if(recurso is null) return new NotFound("No se encontro el recurso");

        recurso.Nombre = ent.nombre;

        return await guardarCambiosAsync();
    }

// -------------------------------------------------------------------------------
    // Editar Descripcion
    public async Task<IResultadoServicio> editarDescripcionAsync(EditarDescripcion ent)
    {
        var recurso = await _db.Recursos.FindAsync(ent.IdRecurso);

        if(recurso is null) return new NotFound("No se encontro el recurso");

        recurso.Descripcion = ent.Descripcion;


        return await guardarCambiosAsync();
    }






    










    public async Task<IResultadoServicio> guardarCambiosAsync()
    {
        if(await _db.SaveChangesAsync() < 0) return new NotFound("Algo salio mal");

        return new Success();
    }
}