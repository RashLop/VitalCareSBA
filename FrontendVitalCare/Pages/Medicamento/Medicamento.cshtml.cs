using FrontendVitalCare.Dto.MedicamentoDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class MedicamentoPageModel : PageModel
    {
        private readonly MedicamentoAdapter _medicamentoAdapter;

        public MedicamentoPageModel(MedicamentoAdapter medicamentoAdapter)
        {
            _medicamentoAdapter = medicamentoAdapter;
        }

        public List<MedicamentoDto> Medicamentos { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        // Obtener lista de medicamentos con filtro opcional
        public async Task OnGetAsync(string filtro = "")
        {
            try
            {
                // Trae todos los medicamentos desde el Adapter
                Medicamentos = await _medicamentoAdapter.GetAllAsync();

                if (!string.IsNullOrEmpty(filtro))
                {
                    Medicamentos = Medicamentos
                        .Where(m =>
                            (!string.IsNullOrEmpty(m.Nombre) && m.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(m.Clasificacion) && m.Clasificacion.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(m.Presentacion) && m.Presentacion.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                        )
                        .ToList();
                }
            }
            catch
            {
                MensajeError = "No se pudieron cargar los medicamentos.";
            }
        }

        // Eliminar un medicamento por id
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            try
            {
                bool exito = await _medicamentoAdapter.DeleteAsync(id);
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