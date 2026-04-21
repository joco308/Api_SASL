using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// variables de coneccion y configuracion
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();


app.Run();