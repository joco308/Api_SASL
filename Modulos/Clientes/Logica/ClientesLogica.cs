using Api_SASL.Modulos.Clientes.DTO;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Modulos.Clientes.Interfaz;
using Api_SASL.Models;
using Microsoft.Extensions.Options;
using Api_SASL.Servicios;
using static BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_SASL.Modulos.Clientes.Logica;

public class ClientesLogica : IClientesLogica
{
    private readonly DevSaslContext _db;
    private readonly IEmailServicio _email;
    private readonly TokenConfiguracion _configuration;
    

    public ClientesLogica(DevSaslContext db, IEmailServicio email,IOptions<TokenConfiguracion> options)
    {
        _db = db;
        _email = email;
        _configuration = options.Value;
    }


    // Mandar factor de autenticacion
    public async Task<IResultadoServicio> manfar2FAAsync(ClienteLogin us)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(u => u.ContactoEmergenciaCorreo == us.correo);

        if (cliente == null || !Verify(us.contraseña, cliente.ContrasenaHash))
        {
            return new NotFound("Credenciales incorrectas.");
        }

        var codigo = new Random().Next(100000, 999999).ToString();
        
        cliente.Codigo2fa = codigo;
        cliente.Expiracion = DateTime.UtcNow.AddMinutes(5);
        cliente.Pediente2fa = true;

        try
        {
            var filasAfectadas = await _db.SaveChangesAsync();

            if(filasAfectadas == 0) return new ValidationError("Algo salio mal");

            await _email.EnviarCodigo2FAAsync(us.correo, codigo);

            return new Success();
        }
        catch (Exception ex)
        {
            return new NotFound($"Error {ex}");
        }
    }

    // verficar factor de autenticacion
    public async Task<IResultadoServicio> verificarCodigo2FAAsyncMandarTokenAsync(Cliente2Fa login)
    {
        // Buscamos al cliente que tiene el proceso pendiente
        var cliente = await _db.Clientes
        .FirstOrDefaultAsync(u => u.ContactoEmergenciaCorreo == login.correo && u.Pediente2fa == true);

        if (cliente == null) return new NotFound("El cliente no tiene un 2FA activo");

        // Validamos si el código expiró
        if (cliente.Expiracion < DateTime.UtcNow)
        {
            // Limpiamos los campos para obligar a pedir uno nuevo
            cliente.Codigo2fa = null;
            cliente.Pediente2fa = false;
            await _db.SaveChangesAsync();
            return new ValidationError("Codigo expiro.");
        }

        // 3. Comparamos los códigos
        if (cliente.Codigo2fa != login.Codigo) return new ValidationError("Codigo incorrecto.");


        //  Creamos los "Claims" (Datos que van dentro del token)
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
            new Claim(ClaimTypes.Email, cliente.ContactoEmergenciaCorreo!),
            new Claim(ClaimTypes.Role, "Cliente")
        };

        //  Generamos la llave de cifrado
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //  Creamos el objeto del Token
        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(8), // El token dura 8 horas
            signingCredentials: creds
        );

        // ¡ÉXITO! Limpiamos el 2FA y permitimos el acceso
        cliente.Codigo2fa = null;
        cliente.Pediente2fa = false;
        await _db.SaveChangesAsync();
        var Token = new JwtSecurityTokenHandler().WriteToken(token);
        return new SuccessWithToken(Token);
    }


    // mostrar info de un cliente
    public async Task<InfoCleinte?> mostrarInfoClienteAsync(int idCliente)
    {
        return await _db.Clientes
                .Where(u => u.IdCliente == idCliente)
                .Select(u => new InfoCleinte(
                    u.IdCliente,
                    u.IdEmpresaNavigation.Detalle,
                    u.NombreCliente,
                    $"Zona {u.IdDireccionNavigation.IdZonaNavigation.Detalle}, Calle {u.IdDireccionNavigation.Calle}, N° {u.IdDireccionNavigation.Ncasa}",
                    u.ContactoEmergenciaCorreo,
                    u.ContrasenaHash,
                    u.Nit
                ))
                .FirstOrDefaultAsync();
    }


    // listar clientes
    public async Task<IEnumerable<InfoClienteCorto>> listarClientesCortoAsync()
    {
        return await _db.Clientes
                .Select(u => new InfoClienteCorto(
                    u.IdCliente,
                    u.NombreCliente,
                    u.Nit
                ))
                .ToArrayAsync();
    }


    // Añadir cliente
    public async Task<IResultadoServicio> añadirClienteAsync(AñadirCliente n)
    {
        SubDominio? nempresa = null;
        var empresa = await _db.SubDominios.FindAsync(n.idEmpresa);
        if(empresa is null)
        {
            if(n.empresa is null) return new ValidationError("Datos mal ingresados falta empresa");

            nempresa = new SubDominio
            {
                IdDominio = 12,
                Detalle = n.empresa
            };

            _db.SubDominios.Add(nempresa);
        }

        SubDominio? nzona = null;
        var Zona = await _db.SubDominios.FindAsync(n.idZona);
        if(Zona is null)
        {
            if(n.Zona is null) return new ValidationError("Datos mal ingresados falta zona");

            nzona = new SubDominio
            {
                IdDominio = 3,
                Detalle = n.Zona
            };

            _db.SubDominios.Add(nzona);
        }

        var direccion = new Direccion
        {
            IdZonaNavigation = Zona??nzona!,
            Calle=n.calle,
            Ncasa=n.ncasa
        };

        _db.Direccions.Add(direccion);

        var hash = HashPassword(n.contraseña);

        var ncliente = new Cliente
        {
            IdEmpresaNavigation = empresa??nempresa!,
            NombreCliente = n.nombreCliente,
            IdDireccionNavigation = direccion,
            ContactoEmergenciaCorreo = n.correo,
            ContrasenaHash = hash,
            Nit =n.nit
        };

        _db.Clientes.Add(ncliente);

        return await guardarDatosDB<Cliente>(ncliente);

    }

















    public async Task<IResultadoServicio> guardarDatosDB() 
    {   
        try 
        {
            var filasAfectadas = await _db.SaveChangesAsync();
            return filasAfectadas > 0 ? new Success() : new NotFound("No se encontró el registro.");
        }
        catch (Exception ex) { return new NotFound($"Error {ex.Message}."); }
    }

    public async Task<IResultadoServicio> guardarDatosDB<T>(T crear) 
    {   
        try 
        {
            var filasAfectadas = await _db.SaveChangesAsync();
            return filasAfectadas > 0 ? new Created<T>(crear) : new NotFound("No se encontró el registro.");
        }
        catch (Exception ex) { return new NotFound($"Error {ex.Message}."); }
    }
}