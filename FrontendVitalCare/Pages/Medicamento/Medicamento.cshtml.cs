using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Services;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class Medicamento : PageModel
    {
        private readonly MedicamentoClient _medicamentoClient;

        public Medicamento(MedicamentoClient medicamentoClient)
        {
            _medicamentoClient = medicamentoClient;
        }

        public List<MedicamentoDto> Medicamentos { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }
        [TempData]
        public string? MensajeError { get; set; }

        public async Task OnGetAsync(string filtro = "")
        {
            try
            {
                Medicamentos = await _medicamentoClient.ObtenerTodosAsync();

                if (!string.IsNullOrEmpty(filtro))
                {
                    Medicamentos = Medicamentos
                    .Where(m =>
                        (!string.IsNullOrEmpty(m.Nombre) && m.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(m.Clasificacion) && m.Clasificacion.Contains(filtro, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(m.Presentacion) && m.Presentacion.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }
            }
            catch
            {
                MensajeError = "No se pudieron cargar los medicamentos.";
            }
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            try
            {
                bool exito = await _medicamentoClient.EliminarAsync(id, 1); // idUsuario ejemplo
                Mensaje = exito ? "Medicamento eliminado correctamente." : "Error al eliminar el medicamento.";
            }
            catch
            {
                MensajeError = "Error al eliminar el medicamento.";
            }

            return RedirectToPage();
        }
    }
}