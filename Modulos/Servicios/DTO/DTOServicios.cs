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
    string Descripcion
    );

public record AsignarMaquinariaServicios(
    int IdServicio,
    int IdMaquinaria,
    int CantidadMaquinaria,
    string DescripcionMaquinaria
);

public record AsignarRecursoServicios(
    int idServicio,
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

public record InfoServicio(
    int IdServicio,
    string NombreEmpresa,
    string NombreCliente,
    string? Contacto,
    int NumeroCasa,
    string Calle,
    string Zona,
    string TipoServicio,
    DateOnly FechaInicio,
    DateOnly? FechaFinal,
    decimal Costo,
    string? Descripcion,
    DateTime Create_at
);

public record AsignarUsuariosServicios(
    int idUsuario,
    int IdServicio,
    TimeOnly HoraDeEntrada,
    TimeOnly HoraDeSalida,
    string DiasLaborales
);