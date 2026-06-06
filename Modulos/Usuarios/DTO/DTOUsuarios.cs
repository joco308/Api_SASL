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

public record PedirDocumentos(int id, int idtipo);

public record DocumentosUsuarioTipo(int idDoc, string nombre, DateOnly fechaSubido);

public record AñadirCapasitacion(
    string Nombre,
    string Descripcion,
    DateOnly Fecha 
);

public record PonerUsuarioCapasitacion(
    int IdUsuario,
    int IdCapacitacion,
    string estado
);

public record ListarCapasitaciones(
    int IdCapacitacion,
    string Nombre,
    string Descripcion,
    DateOnly Fecha,
    int inscritos
);

public record usuarioInscrito(
    int IdUsuario,
    string nombre,
    string estado
);

public record InfoCapasitacion(
    int IdCapacitacion,
    string Nombre,
    string Descripcion,
    DateOnly Fecha,
    usuarioInscrito[] inscritos
);

public record AñadirUniforme(
    string NombreUniforme,
    int Talla,
    string Descripcion
);

public record ListarUniformes(
    int IdUniforme,
    string NombreUniforme,
    int Talla,
    string Descripcion
);

public record AsignarUniformeEmpleado(
    int IdUsuario,
    int IdUniforme,
    DateOnly FechaEntrega,
    DateOnly FechaDevolucion,
    string Estado
);

public record UsuariosUniformes(
    int IdAsignacionUniforme,
    string NombreEmpleado,
    string NombreUniforme,
    int Talla,
    DateOnly FechaEntrega,
    DateOnly? FechaDevolucion
);



// ========================================================================
//para manejar catalo nada mas!!!!!!!!!!!!!
public record CatalogoDTO(int Id, string Detalle);


