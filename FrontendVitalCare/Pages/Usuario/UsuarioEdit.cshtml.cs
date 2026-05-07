using FrontendVitalCare.Dto.Usuarios;
using FrontendVitalCare.Dto;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Usuario
{
    public class UsuarioEditModel : BasePageModel
    {
        private readonly UsuarioClient _usuarioClient;

        [BindProperty]
        public UsuarioUpdateDto Input { get; set; } = new();

        public UsuarioEditModel(UsuarioClient usuarioClient)
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

        public async Task<IActionResult> OnPostCargarUsuarioParaEdicionAsync(int id)
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            var (resultado, usuario) = await _usuarioClient.ObtenerPorIdAsync(id);
            if (!resultado.Exito || usuario == null)
                return RedirectToPage("Usuario", new { error = resultado.Mensaje });

            Input = new UsuarioUpdateDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Ci = usuario.Ci,
                CiExtencion = usuario.CiExtencion,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Role = usuario.Role,
                Activo = (byte)Math.Max(usuario.Activo, (sbyte)0),
                MustChangePassword = (byte)Math.Max(usuario.MustChangePassword, (sbyte)0)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostActualizarUsuarioAsync()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            OperacionApiDto resultado = await _usuarioClient.ActualizarAsync(Input);
            if (!resultado.Exito)
            {
                Estado.MensajeError = resultado.Mensaje;
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = "Perfil de usuario actualizado correctamente" });
        }
    }
}
