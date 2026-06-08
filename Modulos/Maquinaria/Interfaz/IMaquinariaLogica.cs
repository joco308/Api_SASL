using Api_SASL.Modulos.Maquinaria.DTO;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using System.Collections;

namespace Api_SASL.Modulos.Maquinaria.Interfaz;

public interface IMaquinariaLogica
{
    // Listar Maquinarias (corto)
    Task<IEnumerable<ListarMaquinaria>> listarMaquinariasAsync();

    // Info de una maquinaria por id
    Task<InfoMaquinaria?> informacionMaquinarioAsync(int Id_Maquinaria);

    // Añadir una maquinaria
    Task<IResultadoServicio> añadirMaquinariaAsync(AgregarMaquinaria ent);

    // Añadir una marca de maquinaria
    Task<IResultadoServicio> añadirMarcaMaquinariaAsync(AgragarMarcaMaquinaria enr);

    // Mostrar marcas 
    Task<IEnumerable<MostrarMarcas>> listarMarcasMAquinariaAsync();

    // Mostra estado de calidad
    Task<IEnumerable<Estado>> listarEstadosMacAsync();

    // mostrar datos de una maquinaria por id (vercion corta para trabajadores y clientes)
    Task<InfoResuminaMaquinara?> mostrarInfoResumidaMaquinaria(int IdMaquinaria);

    // añadir mantenimiento y asignarlo a una maquinaria
    Task<IResultadoServicio> manteniminetoMaquinariaAsync(AddManteniminetoMaquinaria ent);

    // mostrar info de 1 mantenimiento
    Task<InfoManteniminto?> mostrarInfoMantenimintoAsync(int IdMantenimiento);

    // listar mantenimintos
    Task<IEnumerable<ListarManteniminto>> ListarMantenimintosAsync();
}