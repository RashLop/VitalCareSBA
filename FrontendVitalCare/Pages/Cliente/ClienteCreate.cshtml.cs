using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto;
using Microsoft.AspNetCore.Mvc;
using FrontendVitalCare.Pages.Base;

namespace FrontendVitalCare.Pages
{
    public class ClienteCreateModel : BasePageModel
    {
        private readonly ClienteApiAdapter clienteApiAdapter;

        [BindProperty]
        public ClienteFormularioDto Cliente { get; set; } = new();

        public string MensajeError { get; set; } = string.Empty;

        public ClienteCreateModel(ClienteApiAdapter clienteApiAdapter)
        {
            this.clienteApiAdapter = clienteApiAdapter;
        }

        public void OnGet()
        {
            Cliente.EsConsumidorFinal = false;
        }

        public async Task<IActionResult> OnPostCrearClienteAsync()
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
                return acceso;

            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null || idUsuario == 0)
            {
                MensajeError = "No se encontró el usuario. Por favor, inicia sesión nuevamente.";
                return Page();
            }

            Cliente.IdUsuario = idUsuario.Value;

            OperacionApiDto resultado = await clienteApiAdapter.CrearAsync(Cliente);

            if (!resultado.Exito)
            {
                MensajeError = resultado.Mensaje;
                return Page();
            }

            return RedirectToPage("Cliente", new { mensaje = resultado.Mensaje });
        }
    }
}
