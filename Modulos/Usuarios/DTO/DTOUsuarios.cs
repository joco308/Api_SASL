using Api_SASL.Models;

namespace Api_SASL.Modulos.Usuarios.DTO;

public record UsuarioLogin(String correo, String password);
public record Login2FA(string email, string codigoIngresado);

public record NuevoUsiario(String NombreUsuario, DateOnly FechaNacimiento, String Correo, int IdRol, int IdEstadoCivil, int IdGradoAcademico, int IdGenero, String Calle, int idZona, int NumeroCasa, String Contrasena, int idPais, int CI);

public record EditarDireccion(int CI, int Zona, String Calle, int NumeroCasa);

public record EditarRol(int CI, int Rol);

public record UsuarioDatos(int IdUsuario, String NombreUsuario, int Ci, String correo, String rol, int salario, DateTime creado);

// ========================================================================
//para manejar catalo nada mas!!!!!!!!!!!!!
public record CatalogoDTO(int Id, string Detalle);


