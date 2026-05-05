using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Dto.Auth;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Servicios;

namespace FrontendVitalCare.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly AuthClient _authClient;

        public RegisterModel(AuthClient authClient)
        {
            _authClient = authClient;
        }

        [BindProperty]
        public UsuarioRegistroDto Registro { get; set; } = new();

        public string MensajeError { get; set; } = string.Empty;
        public string MensajeOk { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            OperacionApiDto resultado = await _authClient.RegistrarAsync(Registro);

            if (!resultado.Exito)
            {
                MensajeError = resultado.Mensaje;
                return Page();
            }

            MensajeOk = resultado.Mensaje;
            ModelState.Clear();
            Registro = new UsuarioRegistroDto();
            return Page();
        }
    }
}
