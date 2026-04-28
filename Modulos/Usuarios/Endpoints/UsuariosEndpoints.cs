using Api_SASL.Modulos.Usuarios.Interfaz;
using Api_SASL.Modulos.Usuarios.Logica;
using Api_SASL.Modulos.Usuarios.DTO;
using Api_SASL.Servicios.InterfazServicios;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/Api/Usuarios");


//====================================================================================================
        group.MapGet("/", async (IUsuariosLogica modulo) =>
        {
            var usuarios = await modulo.usuarios();
            
            return Results.Ok(usuarios);
        })
        .WithSummary("Obtiene la lista completa de trabajadores")
        .RequireAuthorization("PersonalAutorizado");


//====================================================================================================
        // iniciar secion y mandar 2FA
        group.MapPost("/solicitar-2fa", async (UsuarioLogin dto, IUsuariosLogica modulo) =>
        {
            var resultado = await modulo.mandar2FA(dto);
            
            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Código enviado al correo" }),
                NotFound n => Results.Unauthorized(),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Pide correo y contraseña y manda al correo 2FA");


//====================================================================================================
        // verificar el 2FA
        group.MapPost("/verificar-2fa", async (Login2FA dto, IUsuariosLogica modulo, HttpContext context) =>
        {
            var resultado = await modulo.verificarCodigo2FAAsyncMandarToken(dto);

            return resultado switch
            {
                SuccessWithToken s => CrearCookieSesion(context, s.Token),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Pide el codifo 2FA y entrega en token en cookes");


//====================================================================================================    
        // mostrar usuarios por servicio
        group.MapGet("/{servicio}" ,async (bool servicio, IUsuariosLogica modulo) =>
        {
            var datos = await modulo.UsuariosFiltados(servicio);

            return datos.Any() 
                ? Results.Ok(datos) 
                : Results.NotFound(new { mensaje = $"algo salio mal" });
        })
        .WithSummary("Obtiene la lista completa de trabajadores con o sin servicio depende a la entrada")
        .RequireAuthorization("PersonalAutorizado");


//====================================================================================================
        // crear usuario
        group.MapPost("/nuevoUsuario", async (NuevoUsiario dto, IUsuariosLogica modulo) =>
        {
            
            var resultado = await modulo.añadirUsuarioAsync(dto);

            // Mapeamos el IResultadoServicio al código HTTP correspondiente
            return resultado switch
            {
                Success => Results.Created($"/api/usuarios/{dto.Correo}", new { mensaje = "Usuario creado exitosamente" }),
                NotFound n => Results.BadRequest(new { error = n.Mensaje }), // Usamos BadRequest porque suele ser un error de datos enviados
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Registra un nuevo trabajador y su dirección en el sistema")
        .RequireAuthorization("PersonalAutorizado");


//====================================================================================================
        // editar direccion
        group.MapPatch("/{ci}/direccion", async (int ci, EditarDireccion dto, IUsuariosLogica modulo) =>
        {
            if (ci != dto.CI) return Results.BadRequest(new { error = "El CI de la URL no coincide con el del cuerpo." });

            var resultado = await modulo.editarUsuarioDireccion(dto);

            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Dirección actualizada correctamente" }),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Actualiza únicamente la ubicación del usuario")
        .RequireAuthorization("PersonalAutorizado"); // Generalmente solo Admin o el propio usuario


//====================================================================================================
        // Editar solo el Rol
        group.MapPatch("/{ci}/rol", async (int ci, EditarRol dto, IUsuariosLogica modulo) =>
        {
            if (ci != dto.CI) return Results.BadRequest(new { error = "El CI de la URL no coincide con el del cuerpo." });

            var resultado = await modulo.editarUsuarioRol(dto);

            return resultado switch
            {
                Success => Results.Ok(new { mensaje = "Rol actualizado correctamente" }),
                ValidationError v => Results.BadRequest(new { error = v.Error }),
                NotFound n => Results.NotFound(new { error = n.Mensaje }),
                _ => Results.StatusCode(500)
            };
        })
        .WithSummary("Cambia el nivel de acceso (Rol) de un trabajador")
        .RequireAuthorization("PersonalAutorizado");

//====================================================================================================
        






    }
    private static IResult CrearCookieSesion(HttpContext context, string token)
    {
        context.Response.Cookies.Append("token_sesion", token, new CookieOptions
        {
            HttpOnly = true,   
            Secure = true,     
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.UtcNow.AddHours(8)
        });

        return Results.Ok(new { mensaje = "Autenticación exitosa" });
    }
}

