using System.Collections.Generic;
using System.Linq;

namespace FrontendVitalCare.Helpers
{
    public static class AuthorizationHelper
    {
        /// <summary>
        /// Verifica si el usuario tiene un rol específico basado en la sesión
        /// </summary>
        public static bool TieneRol(HttpContext context, string rolRequerido)
        {
            string role = context?.Session?.GetString("Role")?.Trim() ?? string.Empty;
            return role.Equals(rolRequerido, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si el usuario tiene uno de los roles especificados
        /// </summary>
        public static bool TieneAlgunRol(HttpContext context, params string[] rolesPermitidos)
        {
            if (rolesPermitidos == null || rolesPermitidos.Length == 0)
                return false;

            string role = context?.Session?.GetString("Role")?.Trim() ?? string.Empty;
            return rolesPermitidos.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        public static bool EstaAutenticado(HttpContext context)
        {
            string? usuario = context?.Session?.GetString("UserName");
            return !string.IsNullOrWhiteSpace(usuario);
        }

        /// <summary>
        /// Obtiene el rol actual del usuario
        /// </summary>
        public static string ObtenerRol(HttpContext context)
        {
            return context?.Session?.GetString("Role")?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el nombre de usuario
        /// </summary>
        public static string ObtenerNombreUsuario(HttpContext context)
        {
            return context?.Session?.GetString("UserName") ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el ID del usuario
        /// </summary>
        public static int? ObtenerIdUsuario(HttpContext context)
        {
            return context?.Session?.GetInt32("IdUsuario");
        }

        /// <summary>
        /// Obtiene el token JWT
        /// </summary>
        public static string ObtenerToken(HttpContext context)
        {
            return context?.Session?.GetString("Token") ?? string.Empty;
        }

        /// <summary>
        /// Verifica si el usuario es administrador
        /// </summary>
        public static bool EsAdmin(HttpContext context)
        {
            return TieneRol(context, "Admin");
        }

        /// <summary>
        /// Verifica si el usuario es bioquímico
        /// </summary>
        public static bool EsBioquimico(HttpContext context)
        {
            return TieneRol(context, "Bioquimico");
        }
    }
}
