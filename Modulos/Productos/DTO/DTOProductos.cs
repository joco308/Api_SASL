namespace Api_SASL.Modulos.Productos.DTO;

public record AñadirRecurso(int IdProvedor, int IdTipo, string nombre, string? Descripcion);

public record ListarRecurso(string NombreProvedor, string EmpresaProvedor, string Tipo, string Nombre, string? Descripcion);

public record EditarNombre(int IdRecurso, string nombre);

public record EditarDescripcion(int IdRecurso, string Descripcion);


