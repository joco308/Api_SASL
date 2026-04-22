namespace Api_SASL.Modulos.Usuarios.DTO;

public record UsuarioLogin(String correo, String password);
public record Login2FA(string email, string codigoIngresado);

