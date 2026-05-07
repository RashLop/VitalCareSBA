using FrontendVitalCare.Dto.ClasificacionDtos;
using Microsoft.AspNetCore.Mvc;
using FrontendVitalCare.Pages.Base;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Clasificacion
{
    public class ClasificacionCreateModel : BasePageModel
    {
        private readonly ClasificacionAdapter _clasificacionAdapter;

        public ClasificacionCreateModel(ClasificacionAdapter clasificacionAdapter)
        {
            _clasificacionAdapter = clasificacionAdapter;
        }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Origen { get; set; } = string.Empty;

        [BindProperty]
        public string Descripcion { get; set; } = string.Empty;

        [TempData]
        public string? MensajeError { get; set; }

        public void OnGet()
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
            {
                return;
            }
        }

        // Crear clasificación
        public async Task<IActionResult> OnPostCrearClasificacionAsync()
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
                return acceso;

            try
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    MensajeError = "El nombre es requerido.";
                    return Page();
                }

                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario == null || idUsuario == 0)
                {
                    MensajeError = "No se encontró el usuario. Por favor, inicia sesión nuevamente.";
                    return Page();
                }

                var nuevaClasificacion = new ClasificacionDto
                {
                    Nombre = Nombre,
                    Origen = Origen,
                    Descripcion = Descripcion,
                    IdUsuario = idUsuario.Value
                };

                var (exito, mensaje) = await _clasificacionAdapter.CreateAsync(nuevaClasificacion);

                if (exito)
                {
                    return RedirectToPage("Clasificacion", new { mensaje = "Clasificación creada correctamente" });
                }
                else
                {
                    MensajeError = mensaje ?? "Error al crear la clasificación.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Ocurrió un error: {ex.Message}";
                return Page();
            }
        }
    }
}
