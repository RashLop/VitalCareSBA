using FrontendVitalCare.Dto;
using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Bioquimico
{
    public class BioquimicoEditModel : BasePageModel
    {
        private readonly UsuarioClient _usuarioClient;

        [BindProperty]
        public UsuarioUpdateDto Input { get; set; } = new()
        {
            Role = "Bioquimico",
            Activo = 1,
            MustChangePassword = 1
        };

        [BindProperty]
        public string CiBase { get; set; } = string.Empty;

        [BindProperty]
        public string CiComplemento { get; set; } = string.Empty;

        public BioquimicoEditModel(UsuarioClient usuarioClient)
        {
            _usuarioClient = usuarioClient;
        }

        public IActionResult OnGet()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            return RedirectToPage("Bioquimico");
        }

        public async Task<IActionResult> OnPostCargarBioquimicoParaEdicionAsync(int id)
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            var (resultado, usuario) = await _usuarioClient.ObtenerPorIdAsync(id);
            if (!resultado.Exito || usuario == null || !EsBioquimico(usuario.Role))
                return RedirectToPage("Bioquimico", new { error = "Bioquimico no encontrado" });

            string ciCompleto = usuario.Ci?.Trim() ?? string.Empty;
            int separador = ciCompleto.IndexOf('-');

            CiBase = separador >= 0 ? ciCompleto[..separador].Trim() : ciCompleto;
            CiComplemento = separador >= 0 ? ciCompleto[(separador + 1)..].Trim() : string.Empty;

            Input = new UsuarioUpdateDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno ?? string.Empty,
                Ci = usuario.Ci ?? string.Empty,
                CiExtencion = usuario.CiExtencion ?? string.Empty,
                Telefono = usuario.Telefono ?? string.Empty,
                Email = usuario.Email ?? string.Empty,
                UserName = usuario.UserName ?? string.Empty,
                Role = "Bioquimico",
                Activo = (byte)Math.Max(usuario.Activo, (sbyte)0),
                MustChangePassword = (byte)Math.Max(usuario.MustChangePassword, (sbyte)0)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostActualizarBioquimicoAsync()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Input.Role = "Bioquimico";
            Input.Ci = ConstruirCi(CiBase, CiComplemento);

            ModelState.Remove("Input.Ci");
            if (!TryValidateModel(Input, nameof(Input)))
            {
                Estado.MensajeError = ObtenerPrimerError() ?? "Verifica los datos del formulario.";
                return Page();
            }

            OperacionApiDto resultado = await _usuarioClient.ActualizarAsync(Input);
            if (!resultado.Exito)
            {
                Estado.MensajeError = resultado.Mensaje;
                return Page();
            }

            return RedirectToPage("Bioquimico", new { mensaje = "Bioquimico actualizado correctamente" });
        }

        private static bool EsBioquimico(string? role)
        {
            return string.Equals(role?.Trim(), "Bioquimico", StringComparison.OrdinalIgnoreCase);
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
