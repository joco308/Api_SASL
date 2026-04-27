namespace Api_SASL.Modulos.Servicios.DTO;

public record AñadirServicio(
    int IdCliente,
    string calle,
    int NumeroCasa,
    int IdZona,
    int IdTipoServicio,
    DateOnly Fechainicio,
    DateOnly FechaFinal,
    decimal costo,
    string Descripcion,
    int IdMaquinaria,
    int CantidadMaquinaria,
    string DescripcionMaquinaria,
    int IdRecurso,
    int CantidadRecursos
    );

public record ListarServicio(
    int IdServicio,
    string Cliente,
    string Direccion,
    string TipoServicio,
    DateOnly FechaInicio,
    DateOnly? FechaFinal,
    decimal costo
);