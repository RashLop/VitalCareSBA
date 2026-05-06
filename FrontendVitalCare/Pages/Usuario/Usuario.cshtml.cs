using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Helpers;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Usuario
{
    public class UsuarioModel : BasePageModel
    {
        private readonly UsuarioClient _usuarioClient;

        public List<UsuarioDto> Usuarios { get; set; } = new();

        public UsuarioModel(UsuarioClient usuarioClient)
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

            await CargarUsuariosAsync(Estado.FiltroActual);
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarUsuarioLogicamenteAsync(int id, string? filtro)
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);

            OperacionApiDto resultado = await _usuarioClient.EliminarAsync(id);
            if (!resultado.Exito)
            {
                Estado.MensajeError = resultado.Mensaje;
                await CargarUsuariosAsync(Estado.FiltroActual);
                return Page();
            }

            return RedirectToPage("Usuario", new
            {
                filtro = Estado.FiltroActual,
                mensaje = "Usuario dado de baja correctamente"
            });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private async Task CargarUsuariosAsync(string filtro)
        {
            var (resultado, usuarios) = await _usuarioClient.ObtenerTodosAsync(filtro);
            Usuarios = usuarios;

            if (!resultado.Exito && string.IsNullOrWhiteSpace(Estado.MensajeError))
                Estado.MensajeError = resultado.Mensaje;
        }
    }
}
