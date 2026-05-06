using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Helpers;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Bioquimico
{
    public class BioquimicoModel : BasePageModel
    {
        private const string RolBioquimico = "Bioquimico";
        private readonly UsuarioClient _usuarioClient;

        public List<UsuarioDto> Bioquimicos { get; set; } = new();

        public BioquimicoModel(UsuarioClient usuarioClient)
        {
            _usuarioClient = usuarioClient;
        }

        public async Task<IActionResult> OnGetAsync(string? filtro, string? mensaje, string? error)
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            CargarParametros(filtro, mensaje, error);

            string errorFiltro = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            if (!string.IsNullOrWhiteSpace(errorFiltro))
            {
                Estado.MensajeError = errorFiltro;
                return Page();
            }

            await CargarBioquimicosAsync(Estado.FiltroActual);
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarBioquimicoLogicamenteAsync(int id, string? filtro)
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);

            OperacionApiDto resultado = await _usuarioClient.EliminarAsync(id);
            if (!resultado.Exito)
            {
                Estado.MensajeError = resultado.Mensaje;
                await CargarBioquimicosAsync(Estado.FiltroActual);
                return Page();
            }

            return RedirectToPage("Bioquimico", new
            {
                filtro = Estado.FiltroActual,
                mensaje = "Bioquímico dado de baja correctamente"
            });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private async Task CargarBioquimicosAsync(string filtro)
        {
            var (resultado, usuarios) = await _usuarioClient.ObtenerTodosAsync(filtro);
            Bioquimicos = usuarios
                .Where(usuario =>
                    string.Equals(usuario.Role?.Trim(), RolBioquimico, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!resultado.Exito && string.IsNullOrWhiteSpace(Estado.MensajeError))
                Estado.MensajeError = resultado.Mensaje;
        }
    }
}
