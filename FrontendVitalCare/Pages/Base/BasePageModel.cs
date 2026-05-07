using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendVitalCare.Pages.Base
{
    public abstract class BasePageModel : PageModel
    {
        public EstadoPagina Estado { get; set; } = new();

        /// <summary>
        /// Valida que el usuario esté autenticado y sea Admin
        /// </summary>
        protected IActionResult? ValidarAccesoAdmin()
        {
            return ValidarAccesoPorRol("Admin");
        }

        /// <summary>
        /// Valida que el usuario esté autenticado y tenga el rol especificado
        /// </summary>
        protected IActionResult? ValidarAccesoPorRol(string rolRequerido)
        {
            string? usuario = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(usuario))
                return RedirectToPage("/Auth/Login");

            string role = HttpContext.Session.GetString("Role")?.Trim() ?? string.Empty;
            if (!role.Equals(rolRequerido, StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/Index");

            return null;
        }

        /// <summary>
        /// Valida que el usuario esté autenticado y tenga uno de los roles especificados
        /// </summary>
        protected IActionResult? ValidarAccesoPorRoles(params string[] rolesPermitidos)
        {
            string? usuario = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(usuario))
                return RedirectToPage("/Auth/Login");

            string role = HttpContext.Session.GetString("Role")?.Trim() ?? string.Empty;
            
            bool tieneAcceso = rolesPermitidos.Any(r => 
                r.Equals(role, StringComparison.OrdinalIgnoreCase));

            if (!tieneAcceso)
                return RedirectToPage("/Index");

            return null;
        }

        /// <summary>
        /// Valida que el usuario esté autenticado
        /// </summary>
        protected IActionResult? ValidarAccesoAutenticado()
        {
            string? usuario = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(usuario))
                return RedirectToPage("/Auth/Login");

            return null;
        }

        /// <summary>
        /// Obtiene el ID del usuario desde la sesión
        /// </summary>
        protected int? ObtenerIdUsuarioSesion()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }

        /// <summary>
        /// Obtiene el rol del usuario desde la sesión
        /// </summary>
        protected string ObtenerRolUsuarioSesion()
        {
            return HttpContext.Session.GetString("Role")?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el nombre de usuario desde la sesión
        /// </summary>
        protected string ObtenerNombreUsuarioSesion()
        {
            return HttpContext.Session.GetString("UserName") ?? string.Empty;
        }

        /// <summary>
        /// Verifica si el usuario actual tiene un rol específico
        /// </summary>
        protected bool TieneRol(string rol)
        {
            string rolActual = ObtenerRolUsuarioSesion();
            return rolActual.Equals(rol, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si el usuario actual tiene uno de los roles especificados
        /// </summary>
        protected bool TieneAlgunRol(params string[] roles)
        {
            string rolActual = ObtenerRolUsuarioSesion();
            return roles.Any(r => r.Equals(rolActual, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtiene el token JWT desde la sesión
        /// </summary>
        protected string ObtenerTokenSesion()
        {
            return HttpContext.Session.GetString("Token") ?? string.Empty;
        }
    }
}
