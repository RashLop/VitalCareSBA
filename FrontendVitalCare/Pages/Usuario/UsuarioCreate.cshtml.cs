using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Helpers;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Usuario
{
    public class UsuarioCreateModel : BasePageModel
    {
        private readonly UsuarioClient _usuarioClient;

        [BindProperty]
        public UsuarioCreateDto Input { get; set; } = new();

        [BindProperty]
        public string CiBase { get; set; } = string.Empty;

        [BindProperty]
        public string CiComplemento { get; set; } = string.Empty;

        public UsuarioCreateModel(UsuarioClient usuarioClient)
        {
            _usuarioClient = usuarioClient;
        }

        public IActionResult OnGet()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            return Page();
        }

        public async Task<IActionResult> OnPostCrearUsuarioAsync()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Input.Ci = ConstruirCi(CiBase, CiComplemento);

            ModelState.Remove("Input.Ci");
            if (!TryValidateModel(Input, nameof(Input)))
            {
                Estado.MensajeError = ObtenerPrimerError() ?? "Verifica los datos del formulario.";
                return Page();
            }

            string userName = CredencialesHelper.GenerarUserName(
                Input.Nombres,
                Input.ApellidoPaterno,
                Input.Ci
            );

            OperacionApiDto resultado = await _usuarioClient.CrearAsync(Input);
            if (!resultado.Exito)
            {
                Estado.MensajeError = resultado.Mensaje;
                return Page();
            }

            return RedirectToPage("Usuario", new
            {
                mensaje = $"Usuario registrado. Username: {userName}. Credenciales enviadas por mail."
            });
        }

        private string ConstruirCi(string ciBase, string ciComplemento)
        {
            string baseLimpia = (ciBase ?? string.Empty).Trim();
            string complementoLimpio = (ciComplemento ?? string.Empty).Trim().ToUpperInvariant();

            return string.IsNullOrWhiteSpace(complementoLimpio)
                ? baseLimpia
                : $"{baseLimpia}-{complementoLimpio}";
        }

        private string? ObtenerPrimerError()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m));
        }
    }
}
