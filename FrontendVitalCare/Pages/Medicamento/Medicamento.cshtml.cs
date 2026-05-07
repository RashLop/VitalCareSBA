using FrontendVitalCare.Dto.MedicamentoDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Adaptadores;
using FrontendVitalCare.Dto.ClasificacionDtos;
using FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class MedicamentoPageModel : PageModel
    {
        private readonly MedicamentoAdapter _medicamentoAdapter;
        private readonly ClasificacionAdapter _clasificacionAdapter;

        public MedicamentoPageModel(MedicamentoAdapter medicamentoAdapter, ClasificacionAdapter clasificacionAdapter)
        {
            _medicamentoAdapter = medicamentoAdapter;
            _clasificacionAdapter = clasificacionAdapter;
        }

        public List<MedicamentoDto> Medicamentos { get; set; } = new();
        public List<ClasificacionDto> Clasificaciones { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        // Obtener lista de medicamentos con filtro opcional
        public async Task OnGetAsync(string filtro = "")
        {
            try
            {
                // Cargar clasificaciones
                Clasificaciones = await _clasificacionAdapter.GetAllAsync();

                // Trae todos los medicamentos desde el Adapter
                Medicamentos = await _medicamentoAdapter.GetAllAsync();

                if (!string.IsNullOrEmpty(filtro))
                {
                    Medicamentos = Medicamentos
                        .Where(m =>
                            (!string.IsNullOrEmpty(m.Nombre) && m.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                            (Clasificaciones.Any(c => c.Id == m.IdClasificacion) && Clasificaciones.First(c => c.Id == m.IdClasificacion).Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(m.Presentacion) && m.Presentacion.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                        )
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error: {ex.Message}";
            }
        }

        // Eliminar un medicamento por id
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            try
            {
                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario == null || idUsuario == 0)
                {
                    MensajeError = "No se encontró el usuario. Por favor, inicia sesión nuevamente.";
                    return RedirectToPage();
                }

                bool exito = await _medicamentoAdapter.DeleteAsync(id, idUsuario.Value);
                Mensaje = exito ? "Medicamento eliminado correctamente." : "Error al eliminar el medicamento.";
            }
            catch
            {
                MensajeError = "Error al eliminar el medicamento.";
            }

            return RedirectToPage(); // Recarga la página
        }
    }
}
