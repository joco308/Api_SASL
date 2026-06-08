using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Maquinaria.DTO;

public record ProvedorInfo(String Nombre, String Empresa, int NIT);
public record MaquinariaMarca(string NombreMarca, string Pais);


public record ListarMaquinaria(int IdMaquinaria, string NombreMaquinaria, string CodigoInventario, string TipoMaquinaria);

public record InfoMaquinaria(int IdMaquinaria, string NombreMaquinaria, string CodigoInventario,ProvedorInfo Provedor, string TipoMaquinaria, string EstadoCalidad, MaquinariaMarca Marca, string? Descripcion);

public record AgregarMaquinaria(string NombreMaquinaria, string CodigoInv, int IdProvedor, int TipoMaquinaria, int EstadoCalidad, int IdMarcaMaquinaria, string Descripcion);

public record AgragarMarcaMaquinaria(int IdPais, string NombreMarca);

public record MostrarMarcas(int IdMarca, string Pais, string NombreMarca);

public record Estado(int IdEstado, string estado);

public record InfoResuminaMaquinara(string NombreMAquinaria, string Marca, string? Descripcion);

public record AddManteniminetoMaquinaria(
    int IdMaquinaria,
    DateOnly FechaMantenimiento,
    string Descripcion,
    decimal Costo
);

public record InfoManteniminto(
    int IdMantenimiento,
    DateOnly FechaMantenimiento,
    string? Descripcion,
    decimal Costo,
    int IdMaquinaria,
    string NombreMaquinaria
);

public record ListarManteniminto(
    int IdMantenimiento,
    decimal Costo,
    string NombreMaquinaria
);