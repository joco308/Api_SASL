using Api_SASL.Models;
using Api_SASL.Modulos.Trabajadores.DTO;
using Api_SASL.Modulos.Trabajadores.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Microsoft.EntityFrameworkCore;

namespace Api_SASL.Modulos.Trabajadores.Logica;

public class TrabajadoresLogica : ITrabajadoresLogica
{
    public readonly DevSaslContext _db;


    public TrabajadoresLogica(DevSaslContext db)
    {
        _db = db;
    }


// ----------------------------------------------------------------------------------
    // Listar Roles
    public async Task<IEnumerable<ListarRoles>> ListarRolesAsync()
    {
        return await _db.Roles
                .Select(
                    u => new ListarRoles(
                        u.IdRol,
                        u.NombreRol,
                        u.Salario
                    )
                )
                .ToArrayAsync();
    }


    // Añadir telefono a trabajador
    public async Task<IResultadoServicio> añadirTelefonosAsync(AñadirTelefonoTrabajadores ent)
    {
        if((ent.idDetalle is null && ent.Detalle is null)||(ent.idDetalle is not null && ent.Detalle is not null)) return new ValidationError("Falta el detalle o id o detalle como string o ambos estan llenos solo necesitas 1");

        var detalle = await _db.SubDominios.FindAsync(ent.idDetalle);
        
        if (detalle is not null)
        {
            var telefono = new TelefonoUsuario
            {
                TelefonoUsuario1 = ent.telefono,
                IdUsuario = ent.idUsuario,
                IdDetalleNavigation = detalle
            }; 

            _db.TelefonoUsuarios.Add(telefono);

            return await guardarCambiosAsync();
        }

        if (ent.Detalle is not null)
        {
            var detallenew = new SubDominio
            {
                IdDominio = 7, //<======= CAMBIAR ACA
                Detalle = ent.Detalle
            };

            _db.SubDominios.Add(detallenew);

            var telefono = new TelefonoUsuario
            {
                TelefonoUsuario1 = ent.telefono,
                IdUsuario = ent.idUsuario,
                IdDetalleNavigation = detallenew
            }; 

            _db.TelefonoUsuarios.Add(telefono);

            return await guardarCambiosAsync();

        }

        return new NotFound("no se encontro el id del Detalle");

    }


    // Ver informacion de 1 usuario
    public async Task<VerInfoUsuarioId?> VerInfoUsuarioAsync(int ci)
    {


        return await _db.UsuarioTrabajadors
                .Where(u => u.Ci == ci)
                .Select(u => new VerInfoUsuarioId(
                    u.IdUsuario,
                    u.IdEstadoCivilNavigation.Detalle,
                    u.IdGradoAcademicoNavigation.Detalle,
                    u.IdGeneroNavigation.Detalle,
                    $"{u.IdDireccionNavigation.Calle} N° {u.IdDireccionNavigation.Ncasa} Zona {u.IdDireccionNavigation.IdZonaNavigation.Detalle}",
                    u.IdRolNavigation.NombreRol,
                    u.IdPaisNavigation.Detalle,
                    u.Correo,
                    u.Ci,
                    u.NombreUsuario,
                    u.FechaNacimiento,
                    u.ServicioAsignado,
                    u.IdSubDominios.Select(t => t.Detalle).ToArray(),
                    _db.TelefonoUsuarios
                        .Where(t => t.IdUsuario == u.IdUsuario)
                        .Select(u => u.TelefonoUsuario1)
                        .ToArray()
                ))
                .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<VerInfoUsuario>> InfoUsuarioCSVAsync()
    {
        return await _db.UsuarioTrabajadors
                .Select(u => new VerInfoUsuario(
                    u.IdUsuario,
                    u.IdEstadoCivilNavigation.Detalle,
                    u.IdGradoAcademicoNavigation.Detalle,
                    u.IdGeneroNavigation.Detalle,
                    $"{u.IdDireccionNavigation.Calle} N° {u.IdDireccionNavigation.Ncasa} Zona {u.IdDireccionNavigation.IdZonaNavigation.Detalle}",
                    u.IdRolNavigation.NombreRol,
                    u.IdPaisNavigation.Detalle,
                    u.Correo,
                    u.Ci,
                    u.NombreUsuario,
                    u.FechaNacimiento,
                    u.ServicioAsignado
                ))
                .ToListAsync();
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