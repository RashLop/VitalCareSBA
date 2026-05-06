using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendVitalCare.Pages
{
    public class ClienteModel : PageModel
    {
        private readonly ClienteApiAdapter clienteApiAdapter;

        public List<ClienteDto> Clientes { get; set; } = new();
        public string FiltroActual { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;

        public ClienteModel(ClienteApiAdapter clienteApiAdapter)
        {
            this.clienteApiAdapter = clienteApiAdapter;
        }

        public async Task OnGetAsync(string? filtro, string? mensaje, string? error)
        {
            FiltroActual = filtro?.Trim() ?? string.Empty;
            Mensaje = mensaje ?? string.Empty;
            MensajeError = error ?? string.Empty;

            try
            {
                Clientes = await clienteApiAdapter.ObtenerTodosAsync(FiltroActual);
            }
            catch (HttpRequestException)
            {
                MensajeError = "No se pudo cargar clientes. Verifica que ServicioVentas este ejecutandose y que la base de datos responda.";
                Clientes = new List<ClienteDto>();
            }
        }

        public async Task<IActionResult> OnPostEliminarClienteLogicamenteAsync(int id)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null || idUsuario == 0)
            {
                return RedirectToPage("Cliente", new { error = "No se encontró el usuario. Por favor, inicia sesión nuevamente." });
            }

            OperacionApiDto resultado = await clienteApiAdapter.EliminarAsync(id, idUsuario: idUsuario.Value);

            if (!resultado.Exito)
                return RedirectToPage("Cliente", new { error = resultado.Mensaje });

            return RedirectToPage("Cliente", new { mensaje = resultado.Mensaje });
        }
    }
}
