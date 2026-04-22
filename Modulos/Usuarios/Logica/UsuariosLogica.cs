using Api_SASL.Models;
using Api_SASL.Servicios;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Modulos.Usuarios.Interfaz;
using Api_SASL.Modulos.Usuarios.DTO;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace Api_SASL.Modulos.Usuarios.Logica;
public class UsuariosLogica : IUsuariosLogica
{
    public readonly DevSaslContext _db;
    public readonly IEmailServicio _email;

    public readonly TokenConfiguracion _configuration;

    public UsuariosLogica(DevSaslContext db, IEmailServicio email, IOptions<TokenConfiguracion> options)
    {
        _db = db;
        _email = email;
        _configuration = options.Value;
    }


// --------------------------------------------------

    // Mandar factor de autenticacion
    public async Task<bool> Mandar2FA(UsuarioLogin us)
    {
        var usuario = await _db.UsuarioTrabajadors.FirstOrDefaultAsync(u => u.Correo == us.correo);

        if (usuario == null || !Verify(us.password, usuario.ContrasenaHash))
        {
            return false;
        }

        var codigo = new Random().Next(100000, 999999).ToString();
        
        usuario.Codigo2fa = codigo;
        usuario.Expiracion = DateTime.UtcNow.AddMinutes(5);
        usuario.Pediente2fa = true;

        await _db.SaveChangesAsync();

        await _email.EnviarCodigo2FAAsync(us.correo, codigo);
        

        return true;
    }  


    // verficar factor de autenticacion
    public async Task<String?> VerificarCodigo2FAAsyncMandarToken(Login2FA login)
    {
        // 1. Buscamos al usuario que tiene el proceso pendiente
        var usuario = await _db.UsuarioTrabajadors
        .Include(u => u.IdRolNavigation)
        .FirstOrDefaultAsync(u => u.Correo == login.email && u.Pediente2fa == true);

        if (usuario == null) return null;

        // 2. Validamos si el código expiró
        if (usuario.Expiracion < DateTime.UtcNow)
        {
            // Limpiamos los campos para obligar a pedir uno nuevo
            usuario.Codigo2fa = null;
            usuario.Pediente2fa = false;
            await _db.SaveChangesAsync();
            return null;
        }

        // 3. Comparamos los códigos
        if (usuario.Codigo2fa != login.codigoIngresado) return null;


        //  Creamos los "Claims" (Datos que van dentro del token)
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.IdRolNavigation.NombreRol)
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
        usuario.Codigo2fa = null;
        usuario.Pediente2fa = false;
        await _db.SaveChangesAsync();
        return new JwtSecurityTokenHandler().WriteToken(token);;
    }


    
} 
