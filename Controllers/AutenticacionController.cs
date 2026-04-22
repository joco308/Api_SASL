using Api_SASL.DTOs;
using Api_SASL.Models;
using Api_SASL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_SASL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacionController : ControllerBase
{
    private readonly DevSaslContext _context;
    private readonly IServicioAutenticacion2FA _servicio2FA;
    private readonly IServicioCorreoElectronico _servicioCorreo;

    public AutenticacionController(
        DevSaslContext context,
        IServicioAutenticacion2FA servicio2FA,
        IServicioCorreoElectronico servicioCorreo)
    {
        _context = context;
        _servicio2FA = servicio2FA;
        _servicioCorreo = servicioCorreo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var usuario = await _context.UsuarioTrabajadors
                .FirstOrDefaultAsync(u => u.Correo == request.Email);

            if (usuario == null)
            {
                return BadRequest(new LoginResponse
                {
                    Mensaje = "Credenciales incorrectas",
                    Estado = "error",
                    Exito = false
                });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.ContrasenaHash))
            {
                return BadRequest(new LoginResponse
                {
                    Mensaje = "Credenciales incorrectas",
                    Estado = "error",
                    Exito = false
                });
            }

            var codigo2FA = _servicio2FA.GenerarCodigo2FA();
            var (_, mensaje) = await _servicio2FA.GuardarCodigo2FAAsync(usuario.IdUsuario, codigo2FA);

            var cuerpoHtml = _servicioCorreo.GenerarCuerpoHtmlCorreo(codigo2FA, usuario.NombreUsuario);
            var correoEnviado = await _servicioCorreo.EnviarCorreoAsync(
                usuario.Correo,
                "Código de Verificación - Sistema SASL",
                cuerpoHtml);

            if (!correoEnviado)
            {
                return StatusCode(500, new LoginResponse
                {
                    Mensaje = "Error al enviar el código de verificación",
                    Estado = "error",
                    Exito = false
                });
            }

            return Ok(new LoginResponse
            {
                Mensaje = "Código de verificación enviado. Por favor, verifica tu correo electrónico.",
                Estado = "pendiente",
                Exito = true
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new LoginResponse
            {
                Mensaje = $"Error interno: {ex.Message}",
                Estado = "error",
                Exito = false
            });
        }
    }

    [HttpPost("verificar-2fa")]
    public async Task<ActionResult<Verificar2FAResponse>> Verificar2FA([FromBody] Verificar2FARequest request)
    {
        try
        {
            var (valido, mensaje, _) = await _servicio2FA.ValidarCodigo2FAAsync(request.Email, request.Codigo);

            if (!valido)
            {
                return BadRequest(new Verificar2FAResponse
                {
                    Mensaje = mensaje,
                    Valido = false
                });
            }

            return Ok(new Verificar2FAResponse
            {
                Mensaje = "Autenticación exitosa",
                Valido = true,
                Token = "token-aqui-generado"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Verificar2FAResponse
            {
                Mensaje = $"Error interno: {ex.Message}",
                Valido = false
            });
        }
    }
}