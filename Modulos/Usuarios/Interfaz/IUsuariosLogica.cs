using Api_SASL.Modulos.Usuarios.DTO;

namespace Api_SASL.Modulos.Usuarios.Interfaz;

public interface IUsuariosLogica
{
    Task<bool> Mandar2FA(UsuarioLogin us);
    Task<String?> VerificarCodigo2FAAsyncMandarToken(Login2FA login);
}