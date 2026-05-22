using Api_SASL.Modulos.Reportes.DTO;
using Api_SASL.Modulos.Reportes.Interfaz;
using Api_SASL.Servicios.InterfazServicios;
using Api_SASL.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Api_SASL.Modulos.Reportes.Logica;

public class ReportesLogica : IReportesLogica
{
    readonly private DevSaslContext _db;

    public ReportesLogica(DevSaslContext db)
    {
        _db = db;
    }


    // agregar incidente
    public async Task<IResultadoServicio> agregarIncidenteAsync(ClaimsPrincipal user, AddIncidente incidente)
    {
        if (user.Identity?.IsAuthenticated != true) return new ValidationError("No tiene permiso");

        var idCliente = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        var servicio = await _db.Servicios.Where(u => u.IdCliente == idCliente).Select(u => u.IdServicio).FirstOrDefaultAsync();

        var nincidente = new Incidente
        {
            IdServicio = servicio,
            Descripcion = incidente.descripcion,
            Fecha = incidente.fecha
        };

        _db.Incidentes.Add(nincidente);
        
        return await guardarDatosDB<Incidente>(nincidente);
    }


    // listar incidente (corto)
    public async Task<IEnumerable<ListaIncidente>> listarIncidenteAsync()
    {
        return await _db.Incidentes
                .Select(u => new ListaIncidente(
                    u.IdIncidente,
                    u.IdServicioNavigation.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                    u.Fecha
                ))
                .ToListAsync();
    }


    // info incidente detallado
    public async Task<infoIncidente?> InfoIncidenteAsync(int idIncidente)
    {
        return await _db.Incidentes
                .Where(u => u.IdIncidente == idIncidente)
                .Select(u => new infoIncidente(
                    u.IdIncidente,
                    u.IdServicioNavigation.IdClienteNavigation.NombreCliente,
                    u.IdServicioNavigation.IdClienteNavigation.IdEmpresaNavigation.Detalle,
                    $"Zona: {u.IdServicioNavigation.IdClienteNavigation.IdDireccionNavigation.IdZonaNavigation.Detalle} Calle: {u.IdServicioNavigation.IdClienteNavigation.IdDireccionNavigation.Calle} N° {u.IdServicioNavigation.IdClienteNavigation.IdDireccionNavigation.Ncasa}",
                    u.IdServicioNavigation.IdClienteNavigation.ContactoEmergencia,
                    _db.TelefonoClientes
                        .Where(t => t.IdCliente == u.IdServicioNavigation.IdClienteNavigation.IdCliente)
                        .Select(t => new TelefonosCliente(
                            t.Telefono,
                            t.IdDetalleNavigation.Detalle
                        ))
                        .ToArray(),
                    u.IdServicioNavigation.TipoServicioNavigation.Detalle,
                    u.Descripcion,
                    u.Fecha
                ))
                .FirstOrDefaultAsync();
    }


    // Agregar un memorandum
    public async Task<IResultadoServicio> agregarMemorandumAsync(AddMemorandum memo)
    {
        var memorial = new Memorial
        {
            IdEmpleado = memo.IdTrabajador,
            Descripcion = memo.Descripcion
        };

        _db.Memorials.Add(memorial);

        return await guardarDatosDB<Memorial>(memorial);
    }


    // Generar Pdf de memorandum
    public async Task<IResultadoServicio> GenerarMemorandoAsync(int idMemo, IWebHostEnvironment env)
    {
        // 1. BLINDAJE DE DB: Cargamos el memorial e incluimos la relación de forma explícita y rápida (.AsNoTracking)
        var memorial = await _db.Memorials
            .Include(m => m.IdEmpleadoNavigation)
            .AsNoTracking() 
            .FirstOrDefaultAsync(m => m.IdMemorial == idMemo); // Cambia IdMemorial por tu nombre de clave primaria real

        if (memorial is null) 
            return new NotFound("No se encontró el registro del memorando en el sistema.");

        if (memorial.IdEmpleadoNavigation is null)
            return new NotFound("El memorando no tiene un empleado asignado válido.");

        // 2. RUTA ABSOLUTA PROFESIONAL: El logo nunca va a fallar, esté donde esté alojado el sistema
        string rutaLogo = Path.Combine(env.WebRootPath, "images", "logo.png");

        try
        {
            // Forzamos la licencia por seguridad antes de compilar el documento
            QuestPDF.Settings.License = LicenseType.Community;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2.5f, Unit.Centimetre); 
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial).FontSize(11).LineHeight(1.5f));

                    // ENCABEZADO
                    page.Header().Column(header =>
                    {
                        header.Item().Row(row =>
                        {
                            var logoCol = row.ConstantItem(3, Unit.Centimetre);
                            
                            // Validación blindada con la ruta absoluta
                            if (File.Exists(rutaLogo))
                            {
                                logoCol.Image(rutaLogo);
                            }
                            else
                            {
                                logoCol.Background(Colors.Grey.Lighten3)
                                    .Padding(5)
                                    .Text("[ LOGO EMPRESA ]").FontSize(9).AlignCenter();
                            }

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("MARKA CH’UXÑA MULTISERVICIOS S.R.L.")
                                    .Bold().FontSize(13).FontColor(Colors.BlueGrey.Darken3).AlignRight();
                                col.Item().Text("Oficina Central y Administración")
                                    .FontSize(9).FontColor(Colors.Grey.Darken2).AlignRight();
                            });
                        });

                        header.Item().PaddingTop(15).PaddingBottom(15)
                            .Background(Colors.Grey.Lighten1).Height(1);
                    });

                    // CONTENIDO
                    page.Content().Column(content =>
                    {
                        content.Item().PaddingBottom(20)
                            .Text("MEMORANDO")
                            .Bold().FontSize(18).FontColor(Colors.BlueGrey.Darken4).AlignCenter();

                        content.Item().PaddingBottom(25).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(80);
                                columns.RelativeColumn(); 
                            });

                            table.Cell().PaddingVertical(3).Text("PARA:").Bold().FontColor(Colors.Grey.Darken3);
                            // Esto ya no romperá porque usamos .Include() arriba
                            table.Cell().PaddingVertical(3).Text(memorial.IdEmpleadoNavigation.NombreUsuario).Bold();

                            table.Cell().PaddingVertical(3).Text("DE:").Bold().FontColor(Colors.Grey.Darken3);
                            table.Cell().PaddingVertical(3).Text("MARKA CH’UXÑA MULTISERVICIOS S.R.L.");

                            table.Cell().PaddingVertical(3).Text("FECHA:").Bold().FontColor(Colors.Grey.Darken3);
                            table.Cell().PaddingVertical(3).Text(DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")));

                            table.Cell().PaddingVertical(3).Text("ASUNTO:").Bold().FontColor(Colors.Grey.Darken3);
                            table.Cell().PaddingVertical(3).Text("Memorandum");
                        });

                        content.Item().PaddingBottom(20).Background(Colors.Grey.Lighten2).Height(0.5f);
                        content.Item().Text(memorial.Descripcion).Justify(); 
                    });

                    // PIE DE PÁGINA
                    page.Footer().Column(footer =>
                    {
                        footer.Item().Background(Colors.Grey.Lighten1).Height(1);
                        footer.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().Text("://markachuxna.com | contacto@markachuxna.com")
                                .FontSize(9).FontColor(Colors.Grey.Darken1);
                                
                            row.AutoItem().Text(x =>
                            {
                                x.Span("Página ").FontSize(9).FontColor(Colors.Grey.Darken1);
                                x.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Darken1);
                            });
                        });
                    });
                });
            });

            var pdfBytes = documento.GeneratePdf();
            
            // Retornamos un resultado de éxito especializado en transportar bytes
            return new docCreated(pdfBytes);
        }
        catch (Exception ex)
        {
            return new NotFound($"Error {ex}");
        }
    }


















    public async Task<IResultadoServicio> guardarDatosDB() 
    {   
        try 
        {
            var filasAfectadas = await _db.SaveChangesAsync();
            return filasAfectadas > 0 ? new Success() : new NotFound("No se encontró el registro.");
        }
        catch (Exception ex) { return new NotFound($"Error {ex.Message}."); }
    }

    public async Task<IResultadoServicio> guardarDatosDB<T>(T crear) 
    {   
        try 
        {
            var filasAfectadas = await _db.SaveChangesAsync();
            return filasAfectadas > 0 ? new Created<T>(crear) : new NotFound("No se encontró el registro.");
        }
        catch (Exception ex) { return new NotFound($"Error {ex.Message}."); }
    }
}