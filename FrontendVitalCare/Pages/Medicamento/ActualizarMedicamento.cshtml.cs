using FrontendVitalCare.Dto.MedicamentoDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class ActualizarMedicamentoModel : PageModel
    {
        private readonly MedicamentoAdapter _medicamentoAdapter;

        public ActualizarMedicamentoModel(MedicamentoAdapter medicamentoAdapter)
        {
            _medicamentoAdapter = medicamentoAdapter;
        }

        public MedicamentoDto? Medicamento { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        public string Clasificacion { get; set; } = string.Empty;

        [BindProperty]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        // Cargar medicamento para edición
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Medicamento = await _medicamentoAdapter.GetByIdAsync(id);

                if (Medicamento == null)
                {
                    return RedirectToPage("Medicamento", new { error = "Medicamento no encontrado" });
                }

                // Llenar las propiedades con los datos del medicamento
                Id = Medicamento.Id;
                Nombre = Medicamento.Nombre;
                Presentacion = Medicamento.Presentacion;
                Clasificacion = Medicamento.Clasificacion;
                Concentracion = Medicamento.Concentracion;
                Precio = Medicamento.Precio;
                Stock = Medicamento.Stock;

                return Page();
            }
            catch
            {
                return RedirectToPage("Medicamento", new { error = "Error al cargar el medicamento" });
            }
        }

        // Actualizar medicamento
        public async Task<IActionResult> OnPostActualizarMedicamentoAsync()
        {
            try
            {
                if (Id <= 0)
                {
                    MensajeError = "ID de medicamento inválido.";
                    Medicamento = await _medicamentoAdapter.GetByIdAsync(Id);
                    return Page();
                }

                var medicamentoActualizado = new MedicamentoDto
                {
                    Id = Id,
                    Nombre = Nombre,
                    Presentacion = Presentacion,
                    Clasificacion = Clasificacion,
                    Concentracion = Concentracion,
                    Precio = Precio,
                    Stock = Stock
                };

                bool exito = await _medicamentoAdapter.UpdateAsync(medicamentoActualizado);

                if (exito)
                {
                    return RedirectToPage("Medicamento", new { mensaje = "Medicamento actualizado correctamente" });
                }
                else
                {
                    MensajeError = "Error al actualizar el medicamento.";
                    Medicamento = await _medicamentoAdapter.GetByIdAsync(Id);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Ocurrió un error: {ex.Message}";
                Medicamento = await _medicamentoAdapter.GetByIdAsync(Id);
                return Page();
            }
        }
    }
}
