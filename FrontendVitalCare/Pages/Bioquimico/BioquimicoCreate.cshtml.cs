using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Bioquimico
{
    public class BioquimicoCreateModel : BasePageModel
    {
        private readonly UsuarioClient _usuarioClient;

        [BindProperty]
        public UsuarioCreateDto Registro { get; set; } = new()
        {
            Role = "Bioquimico"
        };

        [BindProperty]
        public string? CiBase { get; set; }

        [BindProperty]
        public string? CiComplemento { get; set; }

        public BioquimicoCreateModel(UsuarioClient usuarioClient)
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

        public async Task<IActionResult> OnPostAsync()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Registro.Role = "Bioquimico";
            Registro.Ci = ConstruirCi(CiBase, CiComplemento);

            OperacionApiDto resultado = await _usuarioClient.CrearAsync(Registro);
            if (!resultado.Exito)
            {
                Estado.MensajeError = resultado.Mensaje;
                return Page();
            }

            Estado.Mensaje = "Usuario registrado correctamente. Revisa las credenciales generadas y tu correo electrónico.";
            Registro = new UsuarioCreateDto { Role = "Bioquimico" };
            CiBase = null;
            CiComplemento = null;
            ModelState.Clear();

            return Page();
        }

        private string ConstruirCi(string? ciBase, string? ciComplemento)
        {
            string baseLimpia = (ciBase ?? string.Empty).Trim();
            string complementoLimpio = (ciComplemento ?? string.Empty).Trim().ToUpperInvariant();

            return string.IsNullOrWhiteSpace(complementoLimpio)
                ? baseLimpia
                : $"{baseLimpia}-{complementoLimpio}";
        }
    }
}
