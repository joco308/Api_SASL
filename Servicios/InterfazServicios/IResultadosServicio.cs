// Definimos una "Interfaz" base para todos los resultados posibles
public interface IResultadoServicio { }

// Definimos los posibles estados como records
public record Success() : IResultadoServicio;
public record NotFound(string Mensaje) : IResultadoServicio;
public record ValidationError(string Error) : IResultadoServicio;
public record SuccessWithToken(string Token) : IResultadoServicio;


