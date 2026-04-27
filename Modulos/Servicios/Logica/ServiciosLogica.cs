using Api_SASL.Modulos.Servicios.DTO;
using Api_SASL.Modulos.Servicios.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Api_SASL.Modulos.Servicios.Logica;
public class ServiciosLogica : IServiciosLogica
{
    public readonly DevSaslContext _db; 

    public ServiciosLogica(DevSaslContext db)
    {
        _db = db;
    }


    // Añadir servicio
    public async Task<IResultadoServicio> añadirServicioNuevoAsync(AñadirServicio ad)
    {
        // creamos la direcion del servicio
        var direccion = new Direccion
        {
            IdZona = ad.IdZona,
            Calle = ad.calle,
            NCasa = ad.NumeroCasa
        };

        _db.Direccions.Add(direccion);


        // creamos el servicio para hacer las relaciones
        var servicio =  new Servicio
        {
            IdCliente = ad.IdCliente,
            IdDireccionNavigation = direccion,
            TipoServicio = ad.IdTipoServicio,
            FechaInicio = ad.Fechainicio,
            FechaFinal = ad.FechaFinal,
            Costo = ad.costo,
            Descripcion = ad.Descripcion
        };

        _db.Servicios.Add(servicio);


        // hacemos relaciones (recursos)
        var asignar_recursos = new AsignacionRecurso
        {
            IdServicioNavigation = servicio,
            IdRecurso = ad.IdRecurso,
            Cantidad = ad.CantidadRecursos,
        };

        _db.AsignacionRecursos.Add(asignar_recursos);


        // hacemos relaciones (maquinaria)
        var asignar_maquinaria =  new AsignacionMaquinarium
        {
            IdServicioNavigation = servicio,
            IdMaquinaria = ad.IdMaquinaria,
            Cantidad = ad.CantidadMaquinaria,
            Descripcion = ad.DescripcionMaquinaria
        };

        _db.AsignacionMaquinaria.Add(asignar_maquinaria);

        if (await _db.SaveChangesAsync()>0)
        {
            return new Success();
        }
        else
        {
            return new NotFound("Algo salio mal..");
        }
    }


    // Mostrar servicios
    public async Task<IEnumerable<ListarServicio>> mostrarServiciosAsync()
    {
        return await _db.Servicios.Select(
            u => new ListarServicio
            (
                u.IdServicio,
                u.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                $"Zona: {u.IdDireccionNavigation.IdZonaNavigation.Detalle} Calle: {u.IdDireccionNavigation.Calle}",
                u.TipoServicioNavigation.Detalle,
                u.FechaInicio,
                u.FechaFinal,
                u.Costo
            ))
            .ToListAsync();
    }


    // Mostrar toos los datos de Servicio 


}