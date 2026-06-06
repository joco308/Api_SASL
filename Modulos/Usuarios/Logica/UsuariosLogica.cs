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
using Microsoft.VisualBasic;

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
            return new NotFound("Credenciales incorrectas.");
        }

        var codigo = new Random().Next(100000, 999999).ToString();
        
        usuario.Codigo2fa = HashPassword(codigo);
        usuario.Expiracion = DateTime.UtcNow.AddMinutes(2);
        usuario.Pediente2fa = true;

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
    public async Task<IResultadoServicio> verificarCodigo2FAAsyncMandarToken(Login2FA login)
    {
        // Buscamos al usuario que tiene el proceso pendiente
        var usuario = await _db.UsuarioTrabajadors
        .Include(u => u.IdRolNavigation)
        .FirstOrDefaultAsync(u => u.Correo == login.email && u.Pediente2fa == true);

        if (usuario == null) return new NotFound("El usuario no tiene un 2FA activo");

        // Validamos si el código expiró
        if (usuario.Expiracion < DateTime.UtcNow)
        {
            // Limpiamos los campos para obligar a pedir uno nuevo
            usuario.Codigo2fa = null;
            usuario.Pediente2fa = false;
            await _db.SaveChangesAsync();
            return new ValidationError("Codigo expiro.");
        }

        // 3. Comparamos los códigos
        if (!Verify(login.codigoIngresado,usuario.Codigo2fa)) return new ValidationError("Codigo incorrecto.");


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
                Ncasa = nv.NumeroCasa
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


    //Editar direccion de un usuario
    public async Task<IResultadoServicio> editarUsuarioDireccion(EditarDireccion ed)
    {
        var usuarioEditado = await _db.UsuarioTrabajadors.Include(u => u.IdDireccionNavigation).FirstOrDefaultAsync(u => u.Ci == ed.CI);

        if (usuarioEditado == null) return new NotFound("No se encontro el Usuario");

        usuarioEditado.IdDireccionNavigation.IdZona = ed.Zona;
        usuarioEditado.IdDireccionNavigation.Calle = ed.Calle;
        usuarioEditado.IdDireccionNavigation.Ncasa = ed.NumeroCasa;

        if(await _db.SaveChangesAsync() > 0)
        {
            return new Success();
        }
        else
        {
            return new NotFound("Algo salio mal");
        }
    }


    //editar rol de usuario
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


    // listar usuarios
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


    // listar usuarios con servicio o sin servicios
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


    // Añadir Documento de Ususrio
    public async Task<IResultadoServicio> subirArchivoUsuarioAsync(IFormFile archivo, IWebHostEnvironment env, DatosParaSubirDoc doc)
    {
        if (archivo is null || archivo.Length == 0)
        {
            return new NotFound("El archivo esta vacio");
        }

        const long tamañoMaximoBytes = 5 * 1024 * 1024; // 5 Megabytes
        if (archivo.Length > tamañoMaximoBytes)
        {
            return new ValidationError("El archivo es demasiado grande");
        }

        var user = await _db.UsuarioTrabajadors.FindAsync(doc.idUSer);
        if (user is null) return new NotFound("No se encontro el usuario"); 


        var extensionPermitidas = ".pdf";
        string nombrelimpio = Path.GetFileName(archivo.FileName);
        string extension = Path.GetExtension(nombrelimpio).ToLowerInvariant();

        if (extensionPermitidas != extension)
        {
            return new NotFound("No se permite ese tipo de archivos");
        }

        try
        {
            SubDominio? tiponew = null;
            var tipo = await _db.SubDominios.FindAsync(doc.idtipoDoc);
            if (tipo is null)
            {
                if(doc.tipoDoc is null) return new NotFound("No se encontro el tipo o falkta el tipo");
                tiponew = new SubDominio
                {
                    IdDominio = 16,
                    Detalle = doc.tipoDoc
                };

                _db.SubDominios.Add(tiponew);
            }

            string carpetaDestino = Path.Combine(env.ContentRootPath, "AlmacenamientoServidor", "Documentos");

            // Nos aseguramos de que la estructura de carpetas exista en el disco duro
            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            // SEGURIDAD: Generar un nombre único e irrepetible para el archivo
            string nombreUnicoArchivo = $"{Guid.NewGuid()}{extension}";
            string rutaCompletaDestino = Path.Combine(carpetaDestino, nombreUnicoArchivo);

            // FLUJO (Stream): Crear la tubería hacia el disco duro y copiar los bytes
            using (var streamDelArchivoFisico = new FileStream(rutaCompletaDestino, FileMode.Create))
            {
                // Transfiere los bytes que vienen de la red directo al almacenamiento
                await archivo.CopyToAsync(streamDelArchivoFisico);
            }

            var documento = new DocumentosUsuario
            {
                IdUsuarioNavigation = user,
                IdTipoDeDocumentoNavigation = tipo ?? tiponew!,
                NombreArchivo = nombreUnicoArchivo,
                FechaSubida = DateOnly.FromDateTime(DateTime.Now),
                UbicacionArchivo = rutaCompletaDestino
            };

            _db.DocumentosUsuarios.Add(documento);

            if (await _db.SaveChangesAsync() == 0) return new NotFound("Algo salio mal");
            return new Created<DocumentosUsuario>(documento);
        }
        catch (Exception ex)
        {
            return new NotFound($"Algo salio mal {ex.Message}");
        }

    }

    // mandar ruta del archivo
    public async Task<IResultadoServicio> mandarRutaDeArchivoAsync(int ent)
    {
        var ruta = await _db.DocumentosUsuarios
            .Where(u => u.IdDocumento == ent)
            .Select(u => u.UbicacionArchivo)
            .FirstOrDefaultAsync();

        if(ruta is null) return new NotFound("No se encontro el archivo");


        return new SuccessM(ruta);
    }

    // añadir carrera universitaria a usuario
    public async Task<IResultadoServicio> añadoirCarreaUniversitariaUsuarioAsync(AñadirCarrera ent)
    {
        SubDominio? ncarrera = null;
        var carrera = await _db.SubDominios.FindAsync(ent.idCarrera);
        if(carrera is null)
        {
            if(ent.Carrera is null) return new ValidationError("Datos mal ingresados ni id ni nueva carrera");

            ncarrera = new SubDominio
            {
                IdDominio = 15,
                Detalle = ent.Carrera
            };

            _db.SubDominios.Add(ncarrera);
        }

        var user = await _db.UsuarioTrabajadors.FindAsync(ent.idUsuario);
        if(user is null) return new NotFound("no existe ese usuario");

        user.IdSubDominios.Add(carrera??ncarrera!);

        if(await _db.SaveChangesAsync() == 0) return new NotFound("Algo salio mal");

        return new Success();

    }

    // Listar archivos por tipo de 1 unuario
    public async Task<IEnumerable<DocumentosUsuarioTipo>> listDocuemntosUsuarioTipoAsync(PedirDocumentos ent)
    {
        return await _db.DocumentosUsuarios
                .AsNoTracking()
                .Where(u => u.IdUsuario == ent.id && u.IdTipoDeDocumento == ent.idtipo)
                .Select(u => new DocumentosUsuarioTipo(
                    u.IdDocumento,
                    u.NombreArchivo,
                    u.FechaSubida
                ))
                .ToListAsync();
    }


    // añadir capasitacion
    public async Task<IResultadoServicio> agregarCapasitacionAsync(AñadirCapasitacion ent)
    {
        var capasitacion = new Capacitacione
        {
            Nombre = ent.Nombre,
            Descripcion = ent.Descripcion,
            Fecha = ent.Fecha
        };

        _db.Capacitaciones.Add(capasitacion);

        if (await _db.SaveChangesAsync() == 0) return new NotFound("Algo salio mal");
        return new Created<Capacitacione>(capasitacion);
    }

    // poner un usuario en capasitacion
    public async Task<IResultadoServicio> usuarioCapasitacionAsync(PonerUsuarioCapasitacion ent)
    {
        var usuariocapasitacion = new UsuariosCapacitacione
        {
            IdUsuario = ent.IdUsuario,
            IdCapacitacion = ent.IdCapacitacion,
            Estado = ent.estado
        };

        _db.UsuariosCapacitaciones.Add(usuariocapasitacion);

        if (await _db.SaveChangesAsync() == 0) return new NotFound("Algo salio mal");
        return new Created<UsuariosCapacitacione>(usuariocapasitacion);
    }


    // listar capasitaciones
    public async Task<IEnumerable<ListarCapasitaciones>> listarCapasitacionesAsync()
    {
        

        return await _db.Capacitaciones
                .Select(u => new ListarCapasitaciones(
                    u.IdCapacitacion,
                    u.Nombre,
                    u.Descripcion!,
                    u.Fecha,
                    _db.UsuariosCapacitaciones
                        .Count(t => t.IdCapacitacion == u.IdCapacitacion)
                ))
                .ToListAsync();
    }


    // informacion de 1 capasitacion
    public async Task<InfoCapasitacion?> InfoCapasitacionAsync(int IdCapacitacion)
    {
        return await _db.Capacitaciones
                .Where(u => u.IdCapacitacion == IdCapacitacion)
                .Select(u => new InfoCapasitacion(
                    u.IdCapacitacion,
                    u.Nombre,
                    u.Descripcion!,
                    u.Fecha,
                    _db.UsuariosCapacitaciones
                        .Where(t => t.IdCapacitacion == u.IdCapacitacion)
                        .Select(t => new usuarioInscrito(
                            t.IdUsuario,
                            t.IdUsuarioNavigation.NombreUsuario,
                            t.Estado
                        ))
                        .ToArray()
                ))
                .FirstOrDefaultAsync();
    }

    // añadir uniforme 
    public async Task<IResultadoServicio> añadirUniformeAsync(AñadirUniforme ent)
    {
        var uniforme = new Uniforme
        {
            NombreUniforme = ent.NombreUniforme,
            Talla = ent.Talla,
            Descripcion = ent.Descripcion
        };

        _db.Uniformes.Add(uniforme);

        if (await _db.SaveChangesAsync() == 0) return new NotFound("Algo salio mal");
        return new Created<Uniforme>(uniforme);
    }

    // listar uniformes
    public async Task<IEnumerable<ListarUniformes>> ListarUniformesAsync()
    {
        return await _db.Uniformes
                .Select(u => new ListarUniformes(
                    u.IdUniforme,
                    u.NombreUniforme,
                    u.Talla,
                    u.Descripcion!
                ))
                .ToListAsync();
    }


    // asignar uniforme a empleado
    public async Task<IResultadoServicio> asignarUniformeAsync(AsignarUniformeEmpleado ent)
    {
        var asginar = new AsignacionUniforme
        {
            IdUsuario = ent.IdUsuario,
            IdUniforme = ent.IdUniforme,
            FechaEntrega= ent.FechaEntrega,
            FechaDevolucion= ent.FechaDevolucion,
            Estado= ent.Estado
        };

        _db.AsignacionUniformes.Add(asginar);

        if (await _db.SaveChangesAsync() == 0) return new NotFound("Algo salio mal");
        return new Created<AsignacionUniforme>(asginar);
    }


    // listar empleados con uniforme
    public async Task<IEnumerable<UsuariosUniformes>> listarEmpleadosUniformeAsync()
    {
        return await _db.AsignacionUniformes
                .Select(u => new UsuariosUniformes(
                    u.IdAsignacionUniforme,
                    u.IdUsuarioNavigation.NombreUsuario,
                    u.IdUniformeNavigation.NombreUniforme,
                    u.IdUniformeNavigation.Talla,
                    u.FechaEntrega,
                    u.FechaDevolucion
                ))
                .ToListAsync();
    }


    // 











    
    









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
