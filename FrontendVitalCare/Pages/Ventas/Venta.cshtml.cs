using Microsoft.AspNetCore.Mvc;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Services;
using FrontendVitalCare.Dto.VentasDtos;

namespace FrontendVitalCare.Pages.Ventas
{
    public class VentaModel : BasePageModel
    {
        private readonly VentaClient _ventaClient;

        public VentaModel(VentaClient ventaClient)
        {
            _ventaClient = ventaClient;
        }

        // Lista de ventas
        public List<VentaDto> Ventas { get; set; } = new();

        // Mensajes
        [TempData]
        public string Mensaje { get; set; } = string.Empty;

        [TempData]
        public string MensajeError { get; set; } = string.Empty;

        // Filtro
        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; } = string.Empty;

        // Cargar lista de ventas
        public async Task OnGetAsync()
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
            {
                await Task.CompletedTask;
                return;
            }

            try
            {
                Ventas = await _ventaClient.ObtenerTodosAsync(Filtro);
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar las ventas: {ex.Message}";
            }
        }

        // Handler para anular venta
        public async Task<IActionResult> OnPostEliminarVentaLogicamenteAsync(int id)
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
                return acceso;

            try
            {
                // IdUsuarioEditor fijo por ejemplo (puedes sacar de sesión)
                bool exito = await _ventaClient.AnularAsync(id, 1);

                Mensaje = exito ? "Venta anulada correctamente." : "Error al anular la venta.";
            }
            catch (Exception ex)
            {
                MensajeError = $"Ocurrió un error: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}