using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Dto.Auth;
using FrontendVitalCare.Helpers;
using FrontendVitalCare.Servicios;

namespace FrontendVitalCare.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly AuthClient _authClient;

        public LoginModel(AuthClient authClient)
        {
            _authClient = authClient;
        }

        [BindProperty]
        public UsuarioLoginRequestDto LoginRequest { get; set; } = new();

        public string MensajeError { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Token")))
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var (resultado, respuesta) = await _authClient.LoginAsync(LoginRequest);

            if (!resultado.Exito || respuesta == null)
            {
                MensajeError = resultado.Mensaje;
                return Page();
            }

            JwtSessionHelper.GuardarSesion(HttpContext, respuesta);
            return RedirectToPage("/Index");
        }
    }
}
