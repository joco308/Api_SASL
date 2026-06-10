namespace Api_SASL.Modulos.Cobros.DTO;

public record CrearCobro(
    int IdServicio,
    int IDQr,
    int IdCliente,
    int DiaMesPagar,
    decimal? Monto
);

public record ListarCobro(
    int IdCobro,
    string NombreCliente,
    string? NombreEmpresa,
    decimal? Monto,
    bool Vigente,
    int DiaMesPagar
);

public record InfoQr(
    int IDQr,
    int IdUsuario,
    string NombreUsuario,
    DateTime FechaEmitida,
    DateTime FechaExpiracion,
    string? Descripcion,
    string consulta = "Consulta Info del qr"
);

public record InfoCobro(
    int IdCobro,
    string NomnreCliente,
    string NombreEmpresa,
    decimal? Monto,
    bool Vigente,
    int DiaMesPagar,
    int Nit,
    string TipoServicio,
    InfoQr? InfoQrCobro
);

public record RegistrarPago(
    int IdCobro,
    string? Descripcion
);

public record ListarPago(
    int IdPago,
    DateTime FechaPago,
    string? Descripcion
);

public record CrearQr(
    string Descripcion,
    IFormFile imgQr,
    DateTime FechaExpiracionQr
);

public record MandarQr(
    FileStream qr
);

public record ListarQr(
    int IdQr,
    string? Descripcion,
    DateTime FechaEmitida
);

public record PagoRealizado(
    int IdCliente,
    int IdCobro
);