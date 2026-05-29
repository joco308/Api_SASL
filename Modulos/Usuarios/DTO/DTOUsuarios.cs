using Api_SASL.Models;

namespace Api_SASL.Modulos.Usuarios.DTO;

public record UsuarioLogin(String correo, String password);
public record Login2FA(string email, string codigoIngresado);

public record NuevoUsiario(
    string NombreUsuario,
    DateOnly FechaNacimiento,
    string Correo,
    int IdRol,
    int IdEstadoCivil,
    int IdGradoAcademico,
    int IdGenero,
    string Calle,
    int idZona,
    int NumeroCasa,
    string Contrasena,
    int idPais,
    int CI);

public record EditarDireccion(int CI, int Zona, String Calle, int NumeroCasa);

public record EditarRol(int CI, int Rol);

public record UsuarioDatos(int IdUsuario, String NombreUsuario, int Ci, String correo, String rol, int salario, DateTime creado);

public record DatosParaSubirDoc(int idUSer, int? idtipoDoc = null, string? tipoDoc = null);

public record AñadirCarrera(int idUsuario, int? idCarrera = null, string? Carrera = null);

public record PedirDocumento(int id, int idtipo);

// ========================================================================
//para manejar catalo nada mas!!!!!!!!!!!!!
public record CatalogoDTO(int Id, string Detalle);


