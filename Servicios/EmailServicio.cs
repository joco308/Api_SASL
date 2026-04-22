using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Servicios;

public class EmailServicio : IEmailServicio 
{
    private readonly SmtpSettings _settings;

    // Inyectamos la configuración desde appsettings.json
    public EmailServicio(IOptions<SmtpSettings> settings) 
    {
        _settings = settings.Value;
    }

    public async Task EnviarCodigo2FAAsync(string emailDestino, string codigo) 
    {
        var message = new MailMessage(_settings.User, emailDestino)
        {
            Subject = "Tu Código de Verificación",
            Body = $"Tu código es: {codigo}. No lo compartas con nadie."
        };

        using var client = new SmtpClient(_settings.Server, _settings.Port);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(_settings.User, _settings.Pass);

        await client.SendMailAsync(message);
    }
}
