namespace Api_SASL.Servicios.InterfazServicios;

public interface IEmailServicio
{
    Task EnviarCodigo2FAAsync(string email, string codigo);
}