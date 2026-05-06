using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendVitalCare.Dto.MedicamentoDtos;
using System.Data;
using VitalCareSBA.FrontendVitalCare.Adaptadores;

namespace FrontendVitalCare.Pages.Medicamento
{
    public class CrearMedicamento : PageModel
    {
        private readonly AdapterJSON<MedicamentoDto> _medicamentoClient;

        public CrearMedicamento(AdapterJSON<MedicamentoDto> medicamentoClient)
        {
            _medicamentoClient = medicamentoClient;
        }

        [BindProperty]
        public MedicamentoDto Medicamento { get; set; } = new MedicamentoDto();

        [TempData]
        public string Mensaje { get; set; } = string.Empty;

        [TempData]
        public string MensajeError { get; set; } = string.Empty;

        public void OnGet()
        {
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