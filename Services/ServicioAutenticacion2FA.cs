using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_SASL.Services;

public class ServicioAutenticacion2FA : IServicioAutenticacion2FA
{
    private readonly DevSaslContext _context;
    private const int DuracionExpiracionMinutos = 5;

    public ServicioAutenticacion2FA(DevSaslContext context)
    {
        _context = context;
    }

    public string GenerarCodigo2FA()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public async Task<(bool Exito, string Mensaje)> GuardarCodigo2FAAsync(int idUsuario, string codigo)
    {
        try
        {
            var usuario = await _context.UsuarioTrabajadors.FindAsync(idUsuario);
            if (usuario == null)
            {
                return (false, "Usuario no encontrado");
            }

            var expiracion = DateTime.UtcNow.AddMinutes(DuracionExpiracionMinutos);

            usuario.Codigo2fa = codigo;
            usuario.Expiracion = expiracion;
            usuario.SegundoFactorPendiente = true;

            await _context.SaveChangesAsync();

            return (true, "Código 2FA guardado correctamente");
        }
        catch (Exception ex)
        {
            return (false, $"Error al guardar el código: {ex.Message}");
        }
    }

    public async Task<(bool Valido, string Mensaje, bool SegundoFactorPendiente)> ValidarCodigo2FAAsync(string email, string codigo)
    {
        try
        {
            var usuario = await _context.UsuarioTrabajadors
                .FirstOrDefaultAsync(u => u.Correo == email);

            if (usuario == null)
            {
                return (false, "Usuario no encontrado", false);
            }

            if (string.IsNullOrEmpty(usuario.Codigo2fa))
            {
                return (false, "No hay código 2FA pendiente", false);
            }

            if (usuario.Expiracion.HasValue && usuario.Expiracion.Value < DateTime.UtcNow)
            {
                usuario.Codigo2fa = null;
                usuario.Expiracion = null;
                usuario.SegundoFactorPendiente = false;
                await _context.SaveChangesAsync();

                return (false, "El código ha expirado", false);
            }

            if (usuario.Codigo2fa != codigo)
            {
                return (false, "Código incorrecto", usuario.SegundoFactorPendiente ?? false);
            }

            usuario.Codigo2fa = null;
            usuario.Expiracion = null;
            usuario.SegundoFactorPendiente = false;
            await _context.SaveChangesAsync();

            return (true, "Verificación exitosa", false);
        }
        catch (Exception ex)
        {
            return (false, $"Error al validar el código: {ex.Message}", false);
        }
    }
}