using Api_SASL.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Api_SASL.Servicios;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Modulos.Usuarios.Interfaz;
using Api_SASL.Modulos.Usuarios.Logica;
using Api_SASL.Modulos.Usuarios.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Api_SASL.Modulos.Servicios.Interfaz;
using Api_SASL.Modulos.Servicios.Logica;
using Api_SASL.Modulos.Servicios.Endpoints;
using Api_SASL.Modulos.Maquinaria.Interfaz;
using Api_SASL.Modulos.Maquinaria.Logica;
using Api_SASL.Modulos.Maquinaria.Endpoints;
using Api_SASL.Modulos.Productos.Endpoints;
using Api_SASL.Modulos.Productos.Interfaz;
using Api_SASL.Modulos.Productos.Logica;
using Api_SASL.Modulos.Provedores.Endpoints;
using Api_SASL.Modulos.Provedores.Logica;
using Api_SASL.Modulos.Provedores.Interfaz;
using Api_SASL.Modulos.Trabajadores.Interfaz;
using Api_SASL.Modulos.Trabajadores.Logica;
using Api_SASL.Modulos.Trabajadores.Endpoints;
using Api_SASL.Modulos.Reportes.Endpoints;
using Api_SASL.Modulos.Reportes.Logica;
using Api_SASL.Modulos.Reportes.Interfaz;
using Api_SASL.Modulos.Clientes.Interfaz;
using Api_SASL.Modulos.Clientes.Logica;
using Api_SASL.Modulos.Clientes.Endpoints;
using QuestPDF.Infrastructure;
using System.Net.WebSockets;

 
// Para los pdf
QuestPDF.Settings.License = LicenseType.Community; 


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var builder = WebApplication.CreateBuilder(args);


// variables de coneccion y configuracion
var conexionbd = builder.Configuration.GetConnectionString("DefaultConnection");
var miReglaCORS = "PermitirNEXTJS";

// Inyeccion de dependencias
builder.Services.AddDbContext<DevSaslContext>(options => options.UseSqlServer(conexionbd));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name:miReglaCORS, policy =>
    {
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// servicios de configuracion smtp y token
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<TokenConfiguracion>(builder.Configuration.GetSection("Jwt"));

// inyectamos sistema de enviar emails
builder.Services.AddScoped<IEmailServicio, EmailServicio>();

// inyectamos modulo usuarios
builder.Services.AddScoped<IUsuariosLogica, UsuariosLogica>();

// inyectamos modulo Servicios
builder.Services.AddScoped<IServiciosLogica, ServiciosLogica>();

// inyectamos modulo Maquinaria
builder.Services.AddScoped<IMaquinariaLogica, MaquinariaLogica>();

// inyectamos modulo productos
builder.Services.AddScoped<IProductosLogica, ProductosLogica>();

// inyectamos modulo provedores
builder.Services.AddScoped<IProvedoresLogica, ProvedoresLogica>();

// inyectamos modulo trabajadores
builder.Services.AddScoped<ITrabajadoresLogica, TrabajadoresLogica>();

// inyectamos modulo reportes
builder.Services.AddScoped<IReportesLogica, ReportesLogica>();

// inyectamos modulo clientes
builder.Services.AddScoped<IClientesLogica, ClientesLogica>();

var jwtKey = builder.Configuration["Jwt:Key"] 
    ?? throw new InvalidOperationException("La clave JWT no está configurada en appsettings.");
// configurar la autenteticacion con el token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = ClaimTypes.Role,
        };
        options.Events = new JwtBearerEvents  {
            OnMessageReceived = context => 
            {
                // Buscamos la cookie que llamamos "token_sesion"
                var token = context.Request.Cookies["token_sesion"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
        
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PersonalAutorizado", policy => 
        policy.RequireRole("Gerente", "Administrador")); 
    options.AddPolicy("Cliente", policy => 
        policy.RequireRole("Cliente"));
});

// webSocket inceatamos la clase
builder.Services.AddSingleton<WebSocketGestor>();


builder.Services.AddOpenApi();

// crear la app
var app = builder.Build();

// añadimos el webSockjet
app.UseWebSockets();

// Configure la app
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseEndpoints();
app.UseHttpsRedirection();
app.UseCors(miReglaCORS);
app.UseAuthentication(); // ¿Quién eres?
app.UseAuthorization();  // ¿Qué puedes hacer?

// Usar archivos de la API 
app.UseStaticFiles();

// Endpoints modulos
app.MapUsuariosEndpoints();
app.MapServiciosEndpoints();
app.MapMaquinariaEndpoints();
app.MapProductosEndpoints();
app.MapProveedoresEndpoints();
app.MapTrabajadoresEndpoints();
app.MapReportesEndpoints();
app.MapClientesEndpoints();


// tener los Sub dominios 
app.MapGet("/Api/Catalogos/{nombre}", async (string nombre, IUsuariosLogica logica) =>
{
    // Normalizamos el nombre (opcional, según tu DB)
    var datos = await logica.ObtenerCatalogoPorDominioAsync(nombre);
    
    return datos.Any() 
        ? Results.Ok(datos) 
        : Results.NotFound(new { mensaje = $"El catálogo '{nombre}' no existe." });
})
.RequireAuthorization()
.WithSummary("Retorna el ID y Nombre de subdominios filtrados por el nombre del Dominio.");


// conectar el webSocket
app.Map("/ws", async (HttpContext context, WebSocketGestor gestor, ClaimsPrincipal user) =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    // 1. El guardia acepta la conexión
    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

    var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    var rol = user.FindFirst(ClaimTypes.Role)?.Value!;

    // 2. El recepcionista lo guarda en su casillero
    gestor.AgregarConexion(usuarioId, rol, webSocket);

    // 3. Mantener la conexión viva escuchando al cliente
    var buffer = new byte[1024 * 4];
    try
    {
        // Bucle de escucha correcto y limpio
        while (webSocket.State == WebSocketState.Open)
        {
            var resultado = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (resultado.MessageType == WebSocketMessageType.Close)
            {
                break;
            }
        }
    }
    catch (WebSocketException ex)
    {
        Console.WriteLine($"[WebSocket] Desconexión abrupta de {usuarioId}: {ex.Message}");
    }
    finally
    {
        // 6. GARANTIZADO: Pase lo que pase, limpiamos el casillero al salir
        await gestor.EliminarConexionAsync(usuarioId, rol);
    }
})
.RequireAuthorization();


#if DEBUG
app.MapGet("/api/diagnostico-final", (ClaimsPrincipal user) =>
{
    return Results.Ok(new {
        Autenticado = user.Identity?.IsAuthenticated,
        Esquema = user.Identity?.AuthenticationType,
        // Asignamos nombres explícitos a cada propiedad
        Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
        EsGerente = user.IsInRole("Gerente"),
        ClaimsCompletos = user.Claims.Select(c => new { c.Type, c.Value }).ToList()
    });
});
#endif

app.Run();