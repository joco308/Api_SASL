namespace Api_SASL.Modulos.Clientes.DTO;

public record ClienteLogin(
    string correo,
    string contraseña
);

public record Cliente2Fa(
    string correo,
    string Codigo
);

public record InfoCleinte(
    int IdCliente,
    string Empresa,
    string nombreCliente,
    string Direccion,
    string? correo,
    string contraseña,
    int nit
);

public record InfoClienteCorto(
    int IdCliente,
    string nombreCliente,
    int nit
);

public record AñadirCliente(
    string nombreCliente,
    string calle,
    int ncasa,
    string correo,
    string contraseña,
    int nit,
    int? idEmpresa = null,
    string? empresa = null,
    int? idZona = null,
    string? Zona = null
);