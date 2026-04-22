namespace Api_SASL.Services;

public interface IServicioCorreoElectronico
{
    Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml);
    string GenerarCuerpoHtmlCorreo(string codigo2FA, string nombreUsuario);
}