using Api_SASL.Modulos.Provedores.DTO;
using Api_SASL.Servicios.InterfazServicios;

namespace Api_SASL.Modulos.Provedores.Interfaz;

public interface IProvedoresLogica
{
    // Listar a provedores
    Task<IEnumerable<ListarProvedores>> ListarProvedoresAsync();

    // Ver informacion de un Provedor
    Task<InformacionProvedor?> InformacionProvedorAsync(int id);

    // Añadir un provedor 
    Task<IResultadoServicio> añadirProvedorAsync(AñadirProvedor ent);

    // Agregar telefono a provedor
    Task<IResultadoServicio> agregarTelefonoProvedor(AgregarTelefonoProvedor ent);

    // Editar provedor
    Task<IResultadoServicio> editarNombreAsync(IdmasNombre ent);
    
}
