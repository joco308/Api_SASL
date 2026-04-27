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
    public async Task<IResultadoServicio> mandar2FA(UsuarioLogin us)
    {
        var usuario = await _db.UsuarioTrabajadors.FirstOrDefaultAsync(u => u.Correo == us.correo);

        if (usuario == null || !Verify(us.password, usuario.ContrasenaHash))
        {
            return new NotFound("No se encontro el usuario o la contraseña es incorrecta");
        }

        var codigo = new Random().Next(100000, 999999).ToString();
        
        usuario.Codigo2fa = codigo;
        usuario.Expiracion = DateTime.UtcNow.AddMinutes(5);
        usuario.Pediente2fa = true;

        await _db.SaveChangesAsync();

        await _email.EnviarCodigo2FAAsync(us.correo, codigo);
        

        return new Success();
    }  


    // verficar factor de autenticacion
    public async Task<IResultadoServicio> verificarCodigo2FAAsyncMandarToken(Login2FA login)
    {
        // Buscamos al usuario que tiene el proceso pendiente
        var usuario = await _db.UsuarioTrabajadors
        .Include(u => u.IdRolNavigation)
        .FirstOrDefaultAsync(u => u.Correo == login.email && u.Pediente2fa == true);

        if (usuario == null) return new NotFound("El usuario no tiene un 2FA activo");

        // 2. Validamos si el código expiró
        if (usuario.Expiracion < DateTime.UtcNow)
        {
            // Limpiamos los campos para obligar a pedir uno nuevo
            usuario.Codigo2fa = null;
            usuario.Pediente2fa = false;
            await _db.SaveChangesAsync();
            return new ValidationError("El tiempo del codigo expiro");
        }

        // 3. Comparamos los códigos
        if (usuario.Codigo2fa != login.codigoIngresado) return new NotFound("Codico incorrecto");


        //  Creamos los "Claims" (Datos que van dentro del token)
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.IdRolNavigation.NombreRol.Trim())
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
        var Token = new JwtSecurityTokenHandler().WriteToken(token);
        return new SuccessWithToken(Token);
    }

// -----------------------------------------------------------------------------

    // Añadir a un usuario

    public async Task<IResultadoServicio> añadirUsuarioAsync(NuevoUsiario nv)
    {
        try
        {
            // Creamos el objeto de Dirección
            var nuevaDireccion = new Direccion
            {
                IdZona = nv.idZona,
                Calle = nv.Calle,
                NCasa = nv.NumeroCasa
            };

            // Hasheamos la contraseña antes de mapear el usuario
            var hash = HashPassword(nv.Contrasena);

            // Creamos el objeto de Usuario
            var nuevoTrabajador = new UsuarioTrabajador
            {
                NombreUsuario = nv.NombreUsuario,
                FechaNacimiento = nv.FechaNacimiento,
                Correo = nv.Correo,
                IdRol = nv.IdRol,
                IdEstadoCivil = nv.IdEstadoCivil,
                IdGradoAcademico = nv.IdGradoAcademico,
                IdGenero = nv.IdGenero,
                IdPais = nv.idPais,
                ContrasenaHash = hash,
                Ci = nv.CI,
                Pediente2fa = false,
                IdDireccionNavigation = nuevaDireccion 
            };

            // Agregamos solo al usuario al contexto. 
            _db.UsuarioTrabajadors.Add(nuevoTrabajador);

            await _db.SaveChangesAsync();
            
            return new Success();
        }
        catch (Exception ex)
        {
            return new NotFound($"Algo salio mal {ex}");
        }
    }

    public async Task<IResultadoServicio> editarUsuarioDireccion(EditarDireccion ed)
    {
        var usuarioEditado = await _db.UsuarioTrabajadors.Include(u => u.IdDireccionNavigation).FirstOrDefaultAsync(u => u.Ci == ed.CI);

        if (usuarioEditado == null) return new NotFound("No se encontro el Usuario");

        usuarioEditado.IdDireccionNavigation.IdZona = ed.Zona;
        usuarioEditado.IdDireccionNavigation.Calle = ed.Calle;
        usuarioEditado.IdDireccionNavigation.NCasa = ed.NumeroCasa;

        if(await _db.SaveChangesAsync() > 0)
        {
            return new Success();
        }
        else
        {
            return new NotFound("Algo salio mal");
        }
    }

    public async Task<IResultadoServicio> editarUsuarioRol(EditarRol ed)
    {
        var usuarioEditado = await _db.UsuarioTrabajadors.FirstOrDefaultAsync(u => u.Ci == ed.CI);

        if(usuarioEditado == null)return new NotFound("No se encontro del Usuario");

        usuarioEditado.IdRol = ed.Rol;

        if(await _db.SaveChangesAsync() > 0)
        {
            return new Success();
        }
        else
        {
            return new NotFound("Algo salio mal");
        }
    }

    public async Task<IEnumerable<UsuarioDatos>> usuarios()
    {
        return await _db.UsuarioTrabajadors
        .AsNoTracking()
        .Select(u => new UsuarioDatos(
            u.IdUsuario,
            u.NombreUsuario,
            u.Ci,
            u.Correo,
            u.IdRolNavigation.NombreRol, 
            u.IdRolNavigation.Salario,
            u.CreateAt
        ))
        .ToListAsync();
    }

    public async Task<IEnumerable<UsuarioDatos>> UsuariosFiltados(bool servicio)
    {
        return await _db.UsuarioTrabajadors
        .AsNoTracking()
        .Where(u => u.ServicioAsignado == servicio)
        .Select(u => new UsuarioDatos(
            u.IdUsuario,
            u.NombreUsuario,
            u.Ci,
            u.Correo,
            u.IdRolNavigation.NombreRol, 
            u.IdRolNavigation.Salario,
            u.CreateAt
        ))
        .ToListAsync();
    }


    
    









//===================================================================================================
// solo para Subdominios
    public async Task<IEnumerable<CatalogoDTO>> ObtenerCatalogoPorDominioAsync(string nombreDominio)
    {
        return await _db.SubDominios
            .AsNoTracking()
            .Where(s => s.IdDominioNavigation.Dominio1 == nombreDominio)
            .Select(s => new CatalogoDTO(
                s.IdSubDominio, 
                s.Detalle
            ))
            .ToListAsync();
    }
} 
