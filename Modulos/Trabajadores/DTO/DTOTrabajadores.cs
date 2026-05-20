namespace Api_SASL.Modulos.Trabajadores.DTO;

public record ListarRoles(int id, string nombre, int salario);

public record AñadirTelefonoTrabajadores(int telefono, int idUsuario, int? idDetalle, string? Detalle);

public record VerInfoUsuario(
    int id,
    string estadocivil,
    string gradoacademico,
    string genero,
    string direccion,
    string Rol,
    string pais,
    string correo,
    int ci,
    string nombre,
    DateOnly fechanacimiento,
    bool ServicioAsignado  
);

public record VerInfoUsuarioId(
    int id,
    string estadocivil,
    string gradoacademico,
    string genero,
    string direccion,
    string Rol,
    string pais,
    string correo,
    int ci,
    string nombre,
    DateOnly fechanacimiento,
    bool ServicioAsignado,
    string[] Carreras,
    int[] telefonos,
    string consultarDocumentos = "/Api/Usuario/VerArchivo/{id:int}/{tipo:int}"
);