using Api_SASL.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Api_SASL.Servicios;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Modulos.Usuarios.Interfaz;
using Api_SASL.Modulos.Usuarios.Logica;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Api_SASL.Modulos.Servicios.Interfaz;
using Api_SASL.Modulos.Servicios.Logica;


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
        .AllowAnyMethod();
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
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
});


builder.Services.AddOpenApi();

// crear la app
var app = builder.Build();

// Configure la app
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseEndpoints();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication(); // ¿Quién eres?
app.UseAuthorization();  // ¿Qué puedes hacer?
app.MapUsuariosEndpoints();


app.MapGet("/{nombre}", async (string nombre, IUsuariosLogica logica) =>
{
    // Normalizamos el nombre (opcional, según tu DB)
    var datos = await logica.ObtenerCatalogoPorDominioAsync(nombre);
    
    return datos.Any() 
        ? Results.Ok(datos) 
        : Results.NotFound(new { mensaje = $"El catálogo '{nombre}' no existe." });
})
.WithSummary("Retorna el ID y Nombre de subdominios filtrados por el nombre del Dominio.");

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

app.Run();