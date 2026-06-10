namespace Api_SASL.Modulos.Reportes.DTO;


public record AddIncidente(
    string descripcion,
    DateOnly fecha
);

public record ListaIncidente(
    int IdIncidente,
    string NombreCliente,
    DateOnly fecha
);

public record infoIncidente(
    int IdIncidente,
    string NombreCliente,
    string Empresa,
    string DireccionServicio,
    string? ContectoEmergencia,
    TelefonosCliente[] Telefonos,
    string TipoServicio,
    string descripcion,
    DateOnly fecha
);

public record TelefonosCliente(
    int telefono,
    string descripcion
);

public record AddMemorandum(
    int IdTrabajador,
    string Descripcion
);


public record NotificarIncidente(
    int idIncidente,
    string descripcionResumina,
    int IdServicio
);

public record EstadoMaquinaria(
    int IdMaquinaria,
    int idEstadoCalidad,
    string? descripcion
);

public record ListHistorialEstadoMaquinaria(
    int IdHistorial,
    int IdMaquinaria,
    string NombreMaquinaria,
    string CodigoINV,
    string EstadoCalidad,
    DateTime FechaCambio,
    string? Descripcion
);