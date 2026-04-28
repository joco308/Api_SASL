using Api_SASL.Modulos.Servicios.DTO;
using Api_SASL.Modulos.Servicios.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

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

        return await guardarDatosDB();
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
    public async Task<InfoServicio?> informacionServicioAsync(int idServicio)
    {
        return await _db.Servicios
            .Where(u => u.IdServicio == idServicio)
            .Select(
            u => new InfoServicio
            (
                u.IdServicio,
                u.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                u.IdClienteNavigation.NombreCliente,
                u.IdClienteNavigation.ContactoEmergencia,
                u.IdDireccionNavigation.NCasa,
                u.IdDireccionNavigation.Calle,
                u.IdDireccionNavigation.IdZonaNavigation.Detalle,
                u.TipoServicioNavigation.Detalle,
                u.FechaInicio,
                u.FechaFinal,
                u.Costo,
                u.Descripcion,
                u.CreateAt        
            ))
            .FirstOrDefaultAsync();
    }


    //Aasignar empleado a Servicio
    public async Task<IResultadoServicio> asignarEmpleadoServicioAsync(AsignarUsuariosServicios entrada)
    {
        var horario = new Horario
        {
            HoraEntrada = entrada.HoraDeEntrada,
            HoraSalida = entrada.HoraDeSalida
        };

        _db.Horarios.Add(horario);

        var dias_laborales = await _db.SubDominios.FirstOrDefaultAsync(u => u.Detalle == entrada.DiasLaborales);
        if(dias_laborales==null) return new ValidationError("Dias laborales mal ingresado");

        var asignacion_empleado = new AsignacionEmpleado
        {
            IdUsuario = entrada.idUsuario,
            IdServicio = entrada.IdServicio,
            IdHorarioNavigation = horario,
            DiasLaboralesNavigation = dias_laborales
        };

        _db.AsignacionEmpleados.Add(asignacion_empleado);

        return await guardarDatosDB();
    }


    // Asignar MAquinaria a Servicio
    public async Task<IResultadoServicio> asignarMaquinariaServicioAsync(AsignarMaquinariaServicios entrada)
    {
        var asignacion_maquinaria = new AsignacionMaquinarium
        {
            IdServicio = entrada.IdServicio,
            IdMaquinaria = entrada.IdMaquinaria,
            Cantidad = entrada.CantidadMaquinaria,
            Descripcion = entrada.DescripcionMaquinaria
        };

        _db.AsignacionMaquinaria.Add(asignacion_maquinaria);

        return await guardarDatosDB();

    }


    // Asignar Recurso a Servicio
    public async Task<IResultadoServicio> asignarRecursoServicioAsync(AsignarRecursoServicios entrada)
    {
        var asignacion_recurso = new AsignacionRecurso
        {
            IdRecurso = entrada.IdRecurso,
            IdServicio = entrada.idServicio,
            Cantidad = entrada.CantidadRecursos
        };

        _db.AsignacionRecursos.Add(asignacion_recurso);


        return await guardarDatosDB();
    }




    public async Task<IResultadoServicio> guardarDatosDB()
    {
        if (await _db.SaveChangesAsync() > 0)
        {
            return new Success();
        }
        else
        {
            return new NotFound("Algo salio mal");
        }
    }
}