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
            Ncasa = ad.NumeroCasa
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
                u.IdDireccionNavigation.Ncasa,
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
        Horario? horarion = null;
        var horario = await _db.Horarios.FindAsync(entrada.idHorario);
        if (horario is null)
        {
            if (entrada.HoraDeEntrada is null && entrada.HoraDeSalida is null) return new ValidationError("Datos mal ingresados fala horario");

            horarion = new Horario
            {
                HoraEntrada = entrada.HoraDeEntrada?? TimeOnly.MinValue,
                HoraSalida = entrada.HoraDeSalida?? TimeOnly.MinValue
            };

            
            _db.Horarios.Add(horarion);
        }

        SubDominio? dias_laboralesn = null;
        var dias_laborales = await _db.SubDominios.FirstOrDefaultAsync(u => u.IdSubDominio == entrada.idDiasLaborales);
        if (dias_laborales == null)
        {
            if(entrada.DiasLaborales is null) return new ValidationError("Dias laborales mal ingresado falta revisar");

            dias_laboralesn = new SubDominio
            {
                IdDominio = 11,
                Detalle = entrada.DiasLaborales
            };

            _db.SubDominios.Add(dias_laboralesn);
        }


        var empleado = await _db.UsuarioTrabajadors.FindAsync(entrada.idUsuario);
        if(empleado == null) return new NotFound("No se encointro el id del Usuario");
        var servicio = await _db.Servicios.FindAsync(entrada.IdServicio);
        if(servicio == null) return new NotFound("No se encontro el servicio");

        empleado.ServicioAsignado = true;

        var asignacion_empleado = new AsignacionEmpleado
        {
            IdUsuario = entrada.idUsuario,
            IdServicio = entrada.IdServicio,
            IdHorarioNavigation = horario??horarion!,
            DiasLaboralesNavigation = dias_laborales??dias_laboralesn!
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


    // Mostrar horarios
    public async Task<IEnumerable<HorarioDTO>> mostrarHorariosAsync()
    {
        return await _db.Horarios
                .Select(u => new HorarioDTO(
                    u.IdHorario,
                    u.HoraEntrada,
                    u.HoraSalida
                ))
                .ToListAsync();
    }


    // mandar datos para descargar csv
    public async Task<IEnumerable<InfoServicio>> datosServicioParaCSVAsync()
    {
        return await _db.Servicios
                .Select(u => new InfoServicio(
                    u.IdServicio,
                    u.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                    u.IdClienteNavigation.NombreCliente,
                    u.IdClienteNavigation.ContactoEmergencia,
                    u.IdDireccionNavigation.Ncasa,
                    u.IdDireccionNavigation.Calle,
                    u.IdDireccionNavigation.IdZonaNavigation.Detalle,
                    u.TipoServicioNavigation.Detalle,
                    u.FechaInicio,
                    u.FechaFinal,
                    u.Costo,
                    u.Descripcion,
                    u.CreateAt        
                ))
                .ToListAsync();
    }
    











    public async Task<IResultadoServicio> guardarDatosDB() 
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