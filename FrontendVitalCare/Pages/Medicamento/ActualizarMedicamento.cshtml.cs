using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Dto.ClasificacionDtos;
using Microsoft.AspNetCore.Mvc;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Adaptadores;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class ActualizarMedicamentoModel : BasePageModel
    {
        private readonly MedicamentoAdapter _medicamentoAdapter;
        private readonly ClasificacionAdapter _clasificacionAdapter;

        public ActualizarMedicamentoModel(MedicamentoAdapter medicamentoAdapter, ClasificacionAdapter clasificacionAdapter)
        {
            _medicamentoAdapter = medicamentoAdapter;
            _clasificacionAdapter = clasificacionAdapter;
        }

        public MedicamentoDto? Medicamento { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        public int IdClasificacion { get; set; }

        [BindProperty]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        public List<ClasificacionDto> Clasificaciones { get; set; } = new();

        [TempData]
        public string? MensajeError { get; set; }

        // Cargar medicamento para edición
        public async Task<IActionResult> OnGetAsync(int id)
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
                return acceso;

            try
            {
                // Cargar clasificaciones
                Clasificaciones = await _clasificacionAdapter.GetAllAsync();

                Medicamento = await _medicamentoAdapter.GetByIdAsync(id);

                if (Medicamento == null)
                {
                    return RedirectToPage("Medicamento", new { error = "Medicamento no encontrado" });
                }

                // Llenar las propiedades con los datos del medicamento
                Id = Medicamento.Id;
                Nombre = Medicamento.Nombre;
                Presentacion = Medicamento.Presentacion;
                IdClasificacion = Medicamento.IdClasificacion;
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

        public async Task<IActionResult> OnPostCargarMedicamentoParaEdicionAsync(int id)
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
                return acceso;

            try
            {
                Clasificaciones = await _clasificacionAdapter.GetAllAsync();

                Medicamento = await _medicamentoAdapter.GetByIdAsync(id);

                if (Medicamento == null)
                {
                    return RedirectToPage("Medicamento", new { error = "Medicamento no encontrado" });
                }

                Id = Medicamento.Id;
                Nombre = Medicamento.Nombre;
                Presentacion = Medicamento.Presentacion;
                IdClasificacion = Medicamento.IdClasificacion;
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

        public async Task<IActionResult> OnPostActualizarMedicamentoAsync()
        {
            IActionResult? acceso = ValidarAccesoPorRoles("Admin", "Bioquimico");
            if (acceso != null)
                return acceso;

            try
            {
                if (Id <= 0)
                {
                    MensajeError = "ID de medicamento inválido.";
                    Medicamento = await _medicamentoAdapter.GetByIdAsync(Id);
                    return Page();
                }

                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario == null || idUsuario == 0)
                {
                    MensajeError = "No se encontró el usuario. Por favor, inicia sesión nuevamente.";
                    Medicamento = await _medicamentoAdapter.GetByIdAsync(Id);
                    return Page();
                }

                var medicamentoActualizado = new MedicamentoDto
                {
                    Id = Id,
                    Nombre = Nombre,
                    Presentacion = Presentacion,
                    IdClasificacion = IdClasificacion,
                    Concentracion = Concentracion,
                    Precio = Precio,
                    Stock = Stock,
                    IdUsuario = idUsuario.Value
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
