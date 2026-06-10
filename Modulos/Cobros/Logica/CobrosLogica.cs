using Api_SASL.Modulos.Cobros.DTO;
using Api_SASL.Modulos.Cobros.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;
using Api_SASL.Servicios;
using System.Security.Claims;

namespace Api_SASL.Modulos.Cobros.Logica;

public class CobrosLogica : ICobrosLogica
{
    private readonly DevSaslContext _db;
    private readonly WebSocketGestor _ws;

    public CobrosLogica(DevSaslContext db, WebSocketGestor ws)
    {
        _db = db;
        _ws = ws;
    }

    // crear cobro
    public async Task<IResultadoServicio> crearCobroAsync(CrearCobro dto)
    {
        var servicio = await _db.Servicios.FindAsync(dto.IdServicio);
        if (servicio is null) return new NotFound("No se encontró el servicio.");

        var cliente = await _db.Clientes.FindAsync(dto.IdCliente);
        if (cliente is null) return new NotFound("No se encontró el cliente.");

        var qr = await _db.Qrs.FindAsync(dto.IDQr);
        if (qr is null) return new NotFound("No se encontro el qr");

        var cobro = new Cobro
        {
            IdServicio = dto.IdServicio,
            IdQr = dto.IDQr,
            IdCliente = dto.IdCliente,
            DiaMesPagar = dto.DiaMesPagar,
            Monto = dto.Monto,
            Vigente = true,
            CreateAt = DateTime.UtcNow
        };

        _db.Cobros.Add(cobro);

        return await guardarCambiosEnviarMensajeAsync<Cobro>(cobro, "Cliente", servicio.IdCliente.ToString(), new { mensaje="Se creo un nuevo cobro para su servicio", IdCobro=cobro });
    }

    // listar cobros
    public async Task<IEnumerable<ListarCobro>> listarCobrosAsync()
    {
        return await _db.Cobros
            .Select(c => new ListarCobro(
                c.IdCobro,
                c.IdClienteNavigation.NombreCliente,
                c.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                c.Monto,
                c.Vigente,
                c.DiaMesPagar
            ))
            .ToListAsync();
    }

    // info de cobro
    public async Task<InfoCobro?> infoCobroAsync(int idCobro)
    {
        return await _db.Cobros
            .Where(c => c.IdCobro == idCobro)
            .Select(c => new InfoCobro(
                c.IdCobro,
                c.IdClienteNavigation.NombreCliente,
                c.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                c.Monto,
                c.Vigente,
                c.DiaMesPagar,
                c.IdClienteNavigation.Nit,
                c.IdServicioNavigation.TipoServicioNavigation.Detalle,
                new InfoQr(
                    c.IdQr,
                    c.IdQrNavigation.IdUsuario,
                    c.IdQrNavigation.IdUsuarioNavigation.NombreUsuario,
                    c.IdQrNavigation.FechaEmitida,
                    c.IdQrNavigation.FechaExpiracion,
                    c.IdQrNavigation.Descripcion
                )      
            ))
            .FirstOrDefaultAsync();
    }

    // registrar pago
    public async Task<IResultadoServicio> registrarPagoAsync(RegistrarPago dto)
    {
        var cobro = await _db.Cobros.FindAsync(dto.IdCobro);
        if (cobro is null)
            return new NotFound("No se encontró el cobro.");

        var pago = new Pago
        {
            IdCobro = dto.IdCobro,
            FechaPago = DateTime.UtcNow,
            Descripcion = dto.Descripcion,
            CreateAt = DateTime.UtcNow
        };

        _db.Pagos.Add(pago);

        return await guardarCambiosAsync<Pago>(pago);
    }

    // listar pagos por cobro
    public async Task<IEnumerable<ListarPago>> listarPagosPorCobroAsync(int idCobro)
    {
        return await _db.Pagos
            .Where(p => p.IdCobro == idCobro)
            .Select(p => new ListarPago(
                p.IdPago,
                p.FechaPago,
                p.Descripcion
            ))
            .ToListAsync();
    }

    // crear qr
    public async Task<IResultadoServicio> crearQrAsync(ClaimsPrincipal user, CrearQr ent, IWebHostEnvironment env)
    {
        var idUsuario = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var usuario = await _db.UsuarioTrabajadors.FindAsync(idUsuario);
        if(usuario is null) return new NotFound("no existe el usuario");

        if(ent.imgQr == null || ent.imgQr.Length == 0) return new ValidationError("El arhivo esta vacio");

        var extencionArchivo = Path.GetExtension(ent.imgQr.FileName);
        string[] extencionesPermitidas = [".png", ".jpg", ".jpng"];

        if (!extencionesPermitidas.Contains(extencionArchivo, StringComparer.OrdinalIgnoreCase)) return new ValidationError("Formato invalido solo se puede subir Imagenes");

        string nombreNuevo = $"{Guid.NewGuid().ToString()}.png";
        string carpetaDestino = Path.Combine(env.ContentRootPath, "AlmacenamientoServidor", "img");

        if (!Directory.Exists(carpetaDestino))
        {
            Directory.CreateDirectory(carpetaDestino);
        }

        string rutaCompletaDestino = Path.Combine(carpetaDestino, nombreNuevo);

        try
        {
            using (var streamDelArchivoFisico = new FileStream(rutaCompletaDestino, FileMode.Create))
            {
                // Transfiere los bytes que vienen de la red directo al almacenamiento
                await ent.imgQr.CopyToAsync(streamDelArchivoFisico);
            }
        }
        catch (Exception)
        {
            return new NotFound("Algo salio muy mal");
        }


        var qr = new Qr
        {
            IdUsuarioNavigation = usuario,
            Descripcion = ent.Descripcion,
            FechaEmitida = DateTime.Now,
            FechaExpiracion = ent.FechaExpiracionQr,
            RutaServidor = nombreNuevo
        };

        _db.Qrs.Add(qr);

        return await guardarCambiosAsync<Qr>(qr);
    }

    // mandar qr
    public async Task<MandarQr?> mandarQrAsync(int IdQr, IWebHostEnvironment env)
    {
        var qr = await _db.Qrs.FindAsync(IdQr);
        if(qr is null) return null;

        string rutacompleta = Path.Combine(env.ContentRootPath, "AlmacenamientoServidor", "img", qr.RutaServidor);

        var stream = new FileStream(rutacompleta, FileMode.Open, FileAccess.Read, FileShare.Read);

        return new MandarQr(stream);
    }

    // listar qrs
    public async Task<IEnumerable<ListarQr>> listarQrsAsync()
    {
        return await _db.Qrs
                .Select(u => new ListarQr(
                    u.IdQr,
                    u.Descripcion,
                    u.FechaEmitida
                ))
                .ToListAsync();
    }

    // listar los cobros de 1 cliente
    public async Task<IEnumerable<ListarCobro>?> listarCobrosPorClienteAsync(ClaimsPrincipal user)
    {
        int IdCliente;
        if(!int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out IdCliente)) return null;
        return await _db.Cobros
        .Where(u => u.IdCliente == IdCliente)
            .Select(c => new ListarCobro(
                c.IdCobro,
                c.IdClienteNavigation.NombreCliente,
                c.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                c.Monto,
                c.Vigente,
                c.DiaMesPagar
            ))
            .ToListAsync();
    }

    // notificar pago realizado
    public async Task<IResultadoServicio> notificarPagoRealizadoAsync(int IdCobro, ClaimsPrincipal user)
    {
        int IdCliente;
        if(!int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out IdCliente)) return new ValidationError("algo anda mal con tu token JWT");

        var dto = new PagoRealizado(IdCliente, IdCobro);
        return await _ws.EnviarMensajeGrupoAsync("Administrador",dto);
    }










    private async Task<IResultadoServicio> guardarCambiosAsync()
    {
        try
        {
            var filas = await _db.SaveChangesAsync();
            return filas > 0
                ? new Success()
                : new NotFound("No se encontró el registro para actualizar.");
        }
        catch (DbUpdateException)
        {
            return new NotFound("Error de validación al guardar en la base de datos.");
        }
    }

    private async Task<IResultadoServicio> guardarCambiosAsync<T>(T obj)
    {
        try
        {
            var filas = await _db.SaveChangesAsync();
            return filas > 0
                ? new Created<T>(obj)
                : new NotFound("No se pudo crear.");
        }
        catch (DbUpdateException)
        {
            return new NotFound("Error de validación al guardar en la base de datos.");
        }
    }
    private async Task<IResultadoServicio> guardarCambiosEnviarMensajeAsync<T>(T obj, string rol, string idUsuario, object mensaje)
    {
        try
        {
            var filas = await _db.SaveChangesAsync();
            if(filas > 0)
            {
                await _ws.EnviarMensajeUserEspesificoAsync(rol, idUsuario, mensaje);
                return new Created<T>(obj);
            }
                return new NotFound("No se pudo crear.");
        }
        catch (DbUpdateException)
        {
            return new NotFound("Error de validación al guardar en la base de datos.");
        }
    }
    
    
}
