using Api_SASL.Models;
using Api_SASL.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var conexionbd = builder.Configuration.GetConnectionString("DefaultConnection");
var miReglaCORS = "PermitirNEXTJS";

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
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServicioCorreoElectronico, ServicioCorreoElectronico>();
builder.Services.AddScoped<IServicioAutenticacion2FA, ServicioAutenticacion2FA>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();


app.Run();