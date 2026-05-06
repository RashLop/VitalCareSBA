using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Dto.VentasDtos;
using FrontendVitalCare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendVitalCare.Pages.Ventas
{
    public class VerVentaModel : PageModel
    {
        private readonly VentaClient _ventaClient;
        private readonly MedicamentoAdapter _medicamentoAdapter;

        public VerVentaModel(VentaClient ventaClient, MedicamentoAdapter medicamentoAdapter)
        {
            _ventaClient = ventaClient;
            _medicamentoAdapter = medicamentoAdapter;
        }

        [BindProperty]
        public VentaDto? Venta { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Venta = await _ventaClient.ObtenerPorIdAsync(id);

                if (Venta == null)
                {
                    MensajeError = "Venta no encontrada";
                    return RedirectToPage("Venta");
                }

                // Cargar nombres de los medicamentos usando el Adapter
                foreach (var detalle in Venta.Detalles)
                {
                    var medicamento = await _medicamentoAdapter.GetByIdAsync(detalle.IdMedicamento);
                    detalle.NombreMedicamento = medicamento?.Nombre ?? "Desconocido";
                }

                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar la venta: {ex.Message}";
                return RedirectToPage("Venta");
            }
        }
    }
}
