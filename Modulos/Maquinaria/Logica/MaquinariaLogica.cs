using Api_SASL.Modulos.Maquinaria.DTO;
using Api_SASL.Modulos.Maquinaria.Interfaz;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Api_SASL.Servicios.InterfazServicios;
using System.Diagnostics.CodeAnalysis;

namespace Api_SASL.Modulos.Maquinaria.MaquinariaLogica;

public class MaquinariaLogica : IMaquinariaLogica
{
    public readonly DevSaslContext _db;

    public MaquinariaLogica(DevSaslContext db)
    {
        _db = db;
    }

// -----------------------------------------------------------------------------
    // listar maquinarias
    public async Task<IEnumerable<ListarMaquinaria>> listarMaquinariasAsync()
    {
        return await _db.Maquinaria
            .Select(
                u => new ListarMaquinaria(
                    u.IdMaquinaria,
                    u.NombreMaquinaria,
                    u.CodigoInv,
                    u.IdTipoMaquinariaNavigation.Detalle
                ))
            .ToListAsync();
    }

// -----------------------------------------------------------------------------
    // Mostrar informaciond e una amquinaria
    public async Task<InfoMaquinaria?> informacionMaquinarioAsync(int Id_Maquinaria)
    {
        return await _db.Maquinaria
            .Where(u => u.IdMaquinaria == Id_Maquinaria)
            .Select(
                u => new InfoMaquinaria(
                    u.IdMaquinaria,
                    u.NombreMaquinaria,
                    u.CodigoInv,
                    new ProvedorInfo(u.IdProveedorNavigation.Nombre,u.IdProveedorNavigation.IdEmpresaNavigation.Detalle,u.IdProveedorNavigation.Nit),
                    u.IdTipoMaquinariaNavigation.Detalle,
                    u.IdEstadoCalidadNavigation.EstadoCalidad1,
                    new MaquinariaMarca(u.IdMarcaMaquinariaNavigation.NombreMarca,u.IdMarcaMaquinariaNavigation.IdPaisNavigation.Detalle),
                    u.Descripcion
                )
            ).FirstOrDefaultAsync();
    }

// -----------------------------------------------------------------------------
// Agragar una maquina
    public async Task<IResultadoServicio> añadirMaquinariaAsync(AgregarMaquinaria ent)
    {
        var provedor = await _db.Provedores.FindAsync(ent.IdProvedor);
        var tipo_maquinaria = await _db.SubDominios.FindAsync(ent.TipoMaquinaria);
        var estado_calida = await _db.EstadoCalidads.FindAsync(ent.EstadoCalidad);
        var id_marca_maquinaria = await _db.MarcaMaquinaria.FindAsync(ent.IdMarcaMaquinaria);

        if(provedor is null || tipo_maquinaria is null || estado_calida is null || id_marca_maquinaria is null) return new NotFound("No se encontro el povedor o el tipo de maquinaria o el estado de caidad o la marca el id es incorrecto");

        var nueva_maquinaria = new Maquinarium
        {
            IdProveedorNavigation =provedor,
            IdTipoMaquinariaNavigation = tipo_maquinaria,
            IdEstadoCalidadNavigation = estado_calida,
            IdMarcaMaquinariaNavigation = id_marca_maquinaria,
            NombreMaquinaria = ent.NombreMaquinaria,
            CodigoInv = ent.CodigoInv,
            Descripcion = ent.Descripcion
        };

        _db.Maquinaria.Add(nueva_maquinaria);

        return await guardarCambiosAsync();
    }

// -----------------------------------------------------------------------------
    // agregar una marca de maquinaria
    public async Task<IResultadoServicio> añadirMarcaMaquinariaAsync(AgragarMarcaMaquinaria ent)
    {
        var pais = await _db.SubDominios.FindAsync(ent.IdPais);
        if(pais is null) return new NotFound("no se encontro el pais");

        var marca = new MarcaMaquinarium
        {
            IdPaisNavigation =pais,
            NombreMarca = ent.NombreMarca 
        };

        _db.MarcaMaquinaria.Add(marca);

        return await guardarCambiosAsync();
    }

// -----------------------------------------------------------------------------
    // Mostrar marcas de maquinaria
    public async Task<IEnumerable<MostrarMarcas>> listarMarcasMAquinariaAsync()
    {
        return await _db.MarcaMaquinaria
            .Select(
                u => new MostrarMarcas(
                    u.IdMarcaMaquinaria,
                    u.IdPaisNavigation.Detalle,
                    u.NombreMarca
                )
            ).ToListAsync();
    }

// -----------------------------------------------------------------------------
    // Listar estados
    public async Task<IEnumerable<Estado>> listarEstadosMacAsync()
    {
        return await _db.EstadoCalidads
            .Select(
                u => new Estado(
                    u.IdEstadoCalidad,
                    u.EstadoCalidad1
                )
            ).ToListAsync();
    }

// -----------------------------------------------------------------------------
    // mostrar resumindo info maquinaria
    public async Task<InfoResuminaMaquinara?> mostrarInfoResumidaMaquinaria(int Id)
    {
        return await _db.Maquinaria
            .Where(u => u.IdMaquinaria == Id)
            .Select(
                u => new InfoResuminaMaquinara(
                    u.NombreMaquinaria,
                    u.IdMarcaMaquinariaNavigation.NombreMarca,
                    u.Descripcion
                )
            ).FirstOrDefaultAsync();
    }
















    public async Task<IResultadoServicio> guardarCambiosAsync() 
    {
        try 
        {
            var filasAfectadas = await _db.SaveChangesAsync();
            
            // Si se modificó al menos una fila, la operación fue un éxito
            if (filasAfectadas > 0)
            {
                return new Success(); 
            }
            
            // Si es 0, no se modificó nada (por ejemplo, el registro no existía)
            return new NotFound("No se encontró el registro para actualizar.");
        }
        catch (DbUpdateException) 
        {
            // Captura errores de claves duplicadas, nulos, o restricciones
            return new NotFound("Error de validación al guardar en la base de datos.");
        }
    }

}