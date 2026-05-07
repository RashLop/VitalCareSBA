using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FrontendVitalCare.Helpers
{
    /// <summary>
    /// Atributo para requerir autenticación en una página
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireAuthenticationAttribute : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            string? usuario = context.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(usuario))
            {
                context.Result = new RedirectToPageResult("/Auth/Login");
                return;
            }

            await next.Invoke();
        }
    }

    /// <summary>
    /// Atributo para requerir un rol específico
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoleAttribute : Attribute, IAsyncPageFilter
    {
        private readonly string _rolRequerido;

        public RequireRoleAttribute(string rolRequerido)
        {
            _rolRequerido = rolRequerido;
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            string? usuario = context.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(usuario))
            {
                context.Result = new RedirectToPageResult("/Auth/Login");
                return;
            }

            string role = context.HttpContext.Session.GetString("Role")?.Trim() ?? string.Empty;
            if (!role.Equals(_rolRequerido, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToPageResult("/Index");
                return;
            }

            await next.Invoke();
        }
    }

    /// <summary>
    /// Atributo para requerir uno de varios roles
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireAnyRoleAttribute : Attribute, IAsyncPageFilter
    {
        private readonly string[] _rolesPermitidos;

        public RequireAnyRoleAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            string? usuario = context.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(usuario))
            {
                context.Result = new RedirectToPageResult("/Auth/Login");
                return;
            }

            string role = context.HttpContext.Session.GetString("Role")?.Trim() ?? string.Empty;
            if (!_rolesPermitidos.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
            {
                context.Result = new RedirectToPageResult("/Index");
                return;
            }

            await next.Invoke();
        }
    }
}
