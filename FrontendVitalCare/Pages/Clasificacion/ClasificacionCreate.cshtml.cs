using FrontendVitalCare.Dto.ClasificacionDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Clasificacion
{
    public class ClasificacionCreateModel : PageModel
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
        }

        // Crear clasificación
        public async Task<IActionResult> OnPostCrearClasificacionAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    MensajeError = "El nombre es requerido.";
                    return Page();
                }

                var nuevaClasificacion = new ClasificacionDto
                {
                    Nombre = Nombre,
                    Origen = Origen,
                    Descripcion = Descripcion
                };

                bool exito = await _clasificacionAdapter.CreateAsync(nuevaClasificacion);

                if (exito)
                {
                    return RedirectToPage("Clasificacion", new { mensaje = "Clasificación creada correctamente" });
                }
                else
                {
                    MensajeError = "Error al crear la clasificación.";
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
