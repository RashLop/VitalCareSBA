using FrontendVitalCare.Services;
using FrontendVitalCare.Dto.VentasDtos;
using FrontendVitalCare.Adaptadores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.MedicamentoDtos;
using System.Text.Json;
using VitalCareSBA.Dto.VentasDtos;
using FrontendVitalCare.Servicios;
using FrontendVitalCare.Dto.PdfDtos;

namespace FrontendVitalCare.Pages.Ventas
{
    public class CrearVentaModel : PageModel
    {
        private readonly ClienteApiAdapter _clienteAdapter;
        private readonly MedicamentoAdapter _medicamentoAdapter;
        private readonly VentaClient _ventaClient;
        private readonly ComprobanteVentaPdfService _pdfService;

        public CrearVentaModel(
            ClienteApiAdapter clienteAdapter,
            MedicamentoAdapter medicamentoAdapter,
            VentaClient ventaClient,
            ComprobanteVentaPdfService pdfService)
        {
            _clienteAdapter = clienteAdapter;
            _medicamentoAdapter = medicamentoAdapter;
            _ventaClient = ventaClient;
            _pdfService = pdfService;
        }

        // Datos para la UI
        public List<ClienteDto> Clientes { get; set; } = new();
        public List<MedicamentoDto> Medicamentos { get; set; } = new();

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        public string MetodoPago { get; set; } = "Efectivo";

        [BindProperty]
        public string DetallesJson { get; set; } = "[]";

        [BindProperty]
        public bool ClienteModalEsConsumidorFinal { get; set; }

        [BindProperty]
        public string ClienteModalNit { get; set; } = string.Empty;

        [BindProperty]
        public string ClienteModalRazonSocial { get; set; } = string.Empty;

        [BindProperty]
        public string ClienteModalCorreoElectronico { get; set; } = string.Empty;

        [TempData]
        public string Mensaje { get; set; } = string.Empty;

        [TempData]
        public string MensajeError { get; set; } = string.Empty;

        // Cargar listas de clientes y medicamentos
        public async Task OnGetAsync()
        {
            Clientes = await _clienteAdapter.ObtenerTodosAsync("");
            Medicamentos = await _medicamentoAdapter.GetAllAsync();

            // Agregar opción "Consumidor Final" si no existe
            var consumidorFinal = Clientes.FirstOrDefault(c => c.EsConsumidorFinal || c.Nit == "CF");
            if (consumidorFinal == null)
            {
                Clientes.Insert(0, new ClienteDto 
                { 
                    IdCliente = -1,
                    RazonSocial = "Consumidor Final", 
                    Nit = "CF",
                    EsConsumidorFinal = true
                });
            }
        }

        // Crear venta
        public async Task<IActionResult> OnPostCrearVentaAsync()
        {
            try
            {
                if (IdCliente <= 0)
                {
                    MensajeError = "Debe seleccionar un cliente válido.";
                    await OnGetAsync();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(MetodoPago))
                {
                    MensajeError = "Debe seleccionar un método de pago.";
                    await OnGetAsync();
                    return Page();
                }

                List<DetalleVentaDto> detalles;
                try
                {
                    var detallesInput = JsonSerializer.Deserialize<List<DetalleVentaInputDto>>(DetallesJson) ?? new();
                    
                    detalles = new List<DetalleVentaDto>();

                    foreach (var item in detallesInput.Where(x => x.IdMedicamento > 0 && x.Cantidad > 0))
                    {
                        var medicamento = await _medicamentoAdapter.GetByIdAsync(item.IdMedicamento);

                        detalles.Add(new DetalleVentaDto
                        {
                            IdMedicamento = item.IdMedicamento,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = medicamento?.Precio ?? 0,
                            NombreMedicamento = medicamento?.Nombre ?? $"Medicamento #{item.IdMedicamento}"
                        });
                    }

                    if (!detalles.Any())
                    {
                        MensajeError = "Debe agregar al menos un medicamento.";
                        await OnGetAsync();
                        return Page();
                    }
                }
                catch
                {
                    MensajeError = "El detalle de la venta no tiene un formato válido.";
                    await OnGetAsync();
                    return Page();
                }

                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

                if (idUsuario == null)
                {
                    MensajeError = "No se pudo identificar el usuario que registra la venta.";
                    await OnGetAsync();
                    return Page();
                }

                var nuevaVenta = new VentaDto
                {
                    IdCliente = IdCliente,
                    IdUsuario = idUsuario.Value,
                    MetodoPago = MetodoPago,
                    Detalles = detalles,
                    FechaHora = DateTime.Now
                };

                bool exito = await _ventaClient.CrearAsync(nuevaVenta);

                if (exito)
                {
                    var cliente = Clientes.FirstOrDefault(c => c.IdCliente == IdCliente);

                    if (cliente == null)
                    {
                        cliente = (await _clienteAdapter.ObtenerTodosAsync(""))
                            .FirstOrDefault(c => c.IdCliente == IdCliente);
                    }

                    var ventas = await _ventaClient.ObtenerTodosAsync("");
                    var ventaCreada = ventas
                        .Where(v => v.IdCliente == IdCliente && v.MetodoPago == MetodoPago)
                        .OrderByDescending(v => v.Id)
                        .FirstOrDefault();

                    var comprobante = new ComprobanteVentaPdfDto
                    {
                        Fecha = ventaCreada?.FechaHora ?? DateTime.Now,
                        Nit = cliente?.Nit ?? "N/A",
                        RazonSocial = cliente?.RazonSocial ?? "Cliente",
                        Cajero = HttpContext.Session.GetString("UserName") ?? "cajero"
                    };

                    foreach (var item in detalles)
                    {
                        comprobante.Detalles.Add(new ComprobanteVentaDetallePdfDto
                        {
                            Cantidad = item.Cantidad,
                            Descripcion = item.NombreMedicamento,
                            PrecioUnitario = item.PrecioUnitario
                        });
                    }

                    comprobante.Total = comprobante.Detalles.Sum(x => x.Importe);

                    byte[] pdf = _pdfService.Generar(comprobante);

                    Response.Headers["Content-Disposition"] = "inline; filename=comprobante.pdf";

                    return File(pdf, "application/pdf");
                }
                else
                {
                    MensajeError = "Error al crear la venta.";
                    await OnGetAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Ocurrió un error: {ex.Message}";
                await OnGetAsync();
                return Page();
            }
        }

        // Crear cliente desde modal
        public async Task<IActionResult> OnPostCrearClienteModalAsync()
        {
            try
            {
                if (ClienteModalEsConsumidorFinal)
                {
                    ClienteModalNit = "CF";
                    ClienteModalRazonSocial = "Consumidor Final";
                }

                if (string.IsNullOrWhiteSpace(ClienteModalNit) || string.IsNullOrWhiteSpace(ClienteModalRazonSocial))
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = "El NIT y la razón social son requeridos."
                    });
                }

                var clienteExistente = Clientes.FirstOrDefault(c => c.Nit == ClienteModalNit);
                if (clienteExistente != null)
                {
                    return new JsonResult(new
                    {
                        success = true,
                        cliente = new
                        {
                            id = clienteExistente.IdCliente,
                            nit = clienteExistente.Nit,
                            razonSocial = clienteExistente.RazonSocial
                        }
                    });
                }

                var clienteFormulario = new ClienteFormularioDto
                {
                    Nit = ClienteModalNit,
                    RazonSocial = ClienteModalRazonSocial,
                    CorreoElectronico = ClienteModalCorreoElectronico
                };

                var resultado = await _clienteAdapter.CrearAsync(clienteFormulario);

                if (resultado.Exito)
                {
                    // Recargar clientes y buscar el que se acaba de crear
                    var clientesActualizados = await _clienteAdapter.ObtenerTodosAsync(ClienteModalNit);
                    var clienteCreado = clientesActualizados.FirstOrDefault(c => c.Nit == ClienteModalNit);

                    if (clienteCreado != null)
                    {
                        return new JsonResult(new
                        {
                            success = true,
                            cliente = new
                            {
                                id = clienteCreado.IdCliente,
                                nit = clienteCreado.Nit,
                                razonSocial = clienteCreado.RazonSocial
                            }
                        });
                    }
                }

                return new JsonResult(new
                {
                    success = false,
                    error = resultado.Mensaje ?? "No se pudo crear el cliente."
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}
