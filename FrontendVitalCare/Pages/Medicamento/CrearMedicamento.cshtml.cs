using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Dto.MedicamentoDtos;
using FrontendVitalCare.Dto.ClasificacionDtos;
using System.Data;
using FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class CrearMedicamento : PageModel
    {
        private readonly AdapterJSON<MedicamentoDto> _medicamentoClient;
        private readonly ClasificacionAdapter _clasificacionAdapter;

        public CrearMedicamento(AdapterJSON<MedicamentoDto> medicamentoClient, ClasificacionAdapter clasificacionAdapter)
        {
            _medicamentoClient = medicamentoClient;
            _clasificacionAdapter = clasificacionAdapter;
        }

        [BindProperty]
        public MedicamentoDto Medicamento { get; set; } = new MedicamentoDto();

        public List<ClasificacionDto> Clasificaciones { get; set; } = new();

        [TempData]
        public string Mensaje { get; set; } = string.Empty;

        [TempData]
        public string MensajeError { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Clasificaciones = await _clasificacionAdapter.GetAllAsync();
        }

        public async Task<IActionResult> OnPostCrearMedicamentoAsync()
        {
            if (!ModelState.IsValid)
            {
                MensajeError = "Por favor, completa todos los campos correctamente.";
                return Page();
            }

            try
            {
                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario == null || idUsuario == 0)
                {
                    MensajeError = "No se encontró el usuario. Por favor, inicia sesión nuevamente.";
                    return Page();
                }

                Medicamento.IdUsuario = idUsuario.Value;

                bool exito = await _medicamentoClient.PostAsync("api/medicamentos", Medicamento);

                if (exito)
                {
                    Mensaje = "Medicamento creado correctamente.";
                    return RedirectToPage("Medicamento");
                }
                else
                {
                    MensajeError = "Error al crear el medicamento.";
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
