using Api_SASL.Modulos.Provedores.DTO;
using Api_SASL.Modulos.Provedores.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Api_SASL.Modulos.Provedores.Logica;

public class ProvedoresLogica : IProvedoresLogica
{
    public readonly DevSaslContext _db;

    public  ProvedoresLogica (DevSaslContext db)
    {
        _db = db;
    }

// --------------------------------------------------------------------------------------
    // Listar Provedores
    public async Task<IEnumerable<ListarProvedores>> ListarProvedoresAsync()
    {   

        var provedores = await _db.Provedores
                .Select(
                    u => new ListarProvedores(
                        u.IdProveedor,
                        u.IdEmpresaNavigation.Detalle,
                        u.Nombre,
                        _db.TelefonoProveedors
                            .Where(t => t.IdProveedor == u.IdProveedor)
                            .Select(t => t.Telefono)
                            .ToArray()
                    )
                )
                .ToArrayAsync();

        return provedores;
    }

// --------------------------------------------------------------------------------------
    // listar informacion de un provedor
    public async Task<InformacionProvedor?> InformacionProvedorAsync(int id)
    {
        var provedorMaquinaria = await _db.Maquinaria
                .Where(u => u.IdProveedor == id)
                .Select(u => new IdmasNombre(u.IdMaquinaria, u.NombreMaquinaria))
                .ToArrayAsync();
        var provedorRecursos = await _db.Recursos
                .Where(u => u.IdProveedor == id)
                .Select(u => new IdmasNombre(u.IdRecurso, u.Nombre))
                .ToArrayAsync();

        var productosProvedor = provedorMaquinaria.Concat(provedorRecursos).ToArray();

        return await _db.Provedores
                .Where(u => u.IdProveedor == id)
                .Select(
                    u => new InformacionProvedor(
                        u.IdEmpresaNavigation.Detalle,
                        productosProvedor,
                        u.Nit,
                        u.Nombre
                    )
                ).FirstOrDefaultAsync();
        
    }

// --------------------------------------------------------------------------------------
    // Añador un provedor
    public async Task<IResultadoServicio> añadirProvedorAsync(AñadirProvedor ent)
    {
        var provedor = new Provedore
        {
            IdEmpresa = ent.IDEmpresa,
            Nit = ent.NIT,
            Nombre = ent.nombre
        };

        _db.Provedores.Add(provedor);

        return await guardarCambiosAsync();
    }

// --------------------------------------------------------------------------------------
    // Agregar telefono a provedor
    public async Task<IResultadoServicio> agregarTelefonoProvedor(AgregarTelefonoProvedor ent)
    {
        var detalle = await _db.SubDominios
                .Where(u => u.Detalle == ent.Detalle)
                .FirstOrDefaultAsync();
        if(detalle is null && ent.Detalle != null)
        {   
            var detallenuevo = new SubDominio
            {
                IdDominio = 5, // <----------- CAMBIAR ESTO FIGATE EL CORRECTO 
                Detalle = ent.Detalle
            };

            _db.SubDominios.Add(detallenuevo);

            var telefono = new TelefonoProveedor
            {
                Telefono = ent.telefono,
                IdDetalleNavigation = detallenuevo,
                IdProveedor = ent.IdProveedor
            };
            _db.TelefonoProveedors.Add(telefono);
        }
        else if(detalle != null)
        {
            var telefono = new TelefonoProveedor
            {
                Telefono = ent.telefono,
                IdDetalleNavigation = detalle,
                IdProveedor = ent.IdProveedor
            };
            _db.TelefonoProveedors.Add(telefono);
        }   

        return await guardarCambiosAsync();

    }

// --------------------------------------------------------------------------------------
    // Editar nombre
    public async Task<IResultadoServicio> editarNombreAsync(IdmasNombre ent)
    {
        var provedor = await _db.Provedores.FindAsync(ent.id);
        if (provedor is null) return new NotFound("No se ecnotro del provedor");

        provedor.Nombre = ent.norbre;

        return await guardarCambiosAsync();
    }











    public async Task<IResultadoServicio> guardarCambiosAsync()
    {
        if(await _db.SaveChangesAsync() < 0) return new NotFound("Algo salio mal");

        return new Success();
    }
}


