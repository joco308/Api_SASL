namespace Api_SASL.Modulos.Productos.DTO;

public record AñadirRecurso(int IdProvedor, int IdTipo, string nombre, string? Descripcion);

public record ListarRecurso(string NombreProvedor, string EmpresaProvedor, string Tipo, string Nombre, string? Descripcion);