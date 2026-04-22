namespace Api_SASL.Services;

public interface IServicioAutenticacion2FA
{
    string GenerarCodigo2FA();
    Task<(bool Exito, string Mensaje)> GuardarCodigo2FAAsync(int idUsuario, string codigo);
    Task<(bool Valido, string Mensaje, bool SegundoFactorPendiente)> ValidarCodigo2FAAsync(string email, string codigo);
}