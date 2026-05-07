using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendVitalCare.Pages
{
    public class ClienteEditModel : PageModel
    {
        private readonly ClienteApiAdapter clienteApiAdapter;

        [BindProperty]
        public ClienteFormularioDto Cliente { get; set; } = new();

        public string MensajeError { get; set; } = string.Empty;

        public ClienteEditModel(ClienteApiAdapter clienteApiAdapter)
        {
            this.clienteApiAdapter = clienteApiAdapter;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ClienteDto? cliente = await clienteApiAdapter.ObtenerPorIdAsync(id);

            if (cliente == null)
                return RedirectToPage("Cliente", new { error = "Cliente no encontrado" });

            Cliente = new ClienteFormularioDto
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = cliente.IdUsuario,
                Estado = cliente.Estado,
                EsConsumidorFinal = cliente.EsConsumidorFinal ||
                    (cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                     cliente.RazonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase)),
                Nit = cliente.Nit,
                RazonSocial = cliente.RazonSocial,
                CorreoElectronico = cliente.CorreoElectronico
            };

            return Page();
        }

        public async Task<IActionResult> OnPostCargarClienteParaEdicionAsync(int id)
        {
            ClienteDto? cliente = await clienteApiAdapter.ObtenerPorIdAsync(id);

            if (cliente == null)
                return RedirectToPage("Cliente", new { error = "Cliente no encontrado" });

            Cliente = new ClienteFormularioDto
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = cliente.IdUsuario,
                Estado = cliente.Estado,
                EsConsumidorFinal = cliente.EsConsumidorFinal ||
                    (cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                     cliente.RazonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase)),
                Nit = cliente.Nit,
                RazonSocial = cliente.RazonSocial,
                CorreoElectronico = cliente.CorreoElectronico
            };

            return Page();
        }

        public async Task<IActionResult> OnPostActualizarClienteAsync()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null || idUsuario == 0)
            {
                MensajeError = "No se encontró el usuario. Por favor, inicia sesión nuevamente.";
                return Page();
            }

            Cliente.IdUsuario = idUsuario.Value;

            OperacionApiDto resultado = await clienteApiAdapter.ActualizarAsync(Cliente.IdCliente, Cliente);

            if (!resultado.Exito)
            {
                MensajeError = resultado.Mensaje;
                return Page();
            }

            return RedirectToPage("Cliente", new { mensaje = resultado.Mensaje });
        }
    }
}
