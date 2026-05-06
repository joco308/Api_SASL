using Api_SASL.Modulos.Productos.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Productos.Interfaz;

public interface IProductosLogica
{
    // Agregar un recurso
    Task<IResultadoServicio> añadirRecursoAsync(AñadirRecurso ent);

    // Listar recurso
    Task<IEnumerable<ListarRecurso>> listarRecursos();

    // Editar nombre
    Task<IResultadoServicio> editarNombreProductoAsync(EditarNombre ent);

    // Editar descripcion 
    Task<IResultadoServicio> editarDescripcionAsync(EditarDescripcion ent);



}