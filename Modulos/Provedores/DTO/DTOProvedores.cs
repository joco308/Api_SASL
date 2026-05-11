using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Provedores.DTO;

public record ListarProvedores(int id, string Empresa, string Nombre, int[] Telefono);

public record InformacionProvedor(
    string Empresa,
    IdmasNombre[]? Productos,
    int Nit,
    string nombre
);

public record AñadirProvedor(
    int IDEmpresa,
    int NIT,
    string nombre
);

public record AgregarTelefonoProvedor(
    int telefono,
    int idDetalle,
    string? Detalle,
    int IdProveedor
);








// ------------------------------------------------------------------------------
public record IdmasNombre(int id, string norbre);