using FrontendVitalCare.Dto.Reportes;
using FrontendVitalCare.Pages.Base;
using FrontendVitalCare.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FrontendVitalCare.Pages.Ventas
{
    public class ReporteVentasPorRolModel : BasePageModel
    {
        private readonly ReporteVentasPorRolClient _reporteClient;

        public ReporteVentasPorRolModel(ReporteVentasPorRolClient reporteClient)
        {
            _reporteClient = reporteClient;
        }

        public ReporteVentasPorRolResponseDto Reporte { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Error { get; set; } = string.Empty;

        public bool TieneDatos => Reporte.Data.Any();

        public async Task<IActionResult> OnGetAsync()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            if (!string.IsNullOrWhiteSpace(Error))
                Estado.MensajeError = Error;

            var (resultado, reporte) = await _reporteClient.ObtenerReporteMensualAsync();
            if (!resultado.Exito || reporte == null)
            {
                if (string.IsNullOrWhiteSpace(Estado.MensajeError))
                    Estado.MensajeError = resultado.Mensaje;
                return Page();
            }

            Reporte = reporte;
            return Page();
        }

        public async Task<IActionResult> OnGetDescargarPdfAsync()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            var (resultado, archivo) = await _reporteClient.DescargarPdfMensualAsync();
            if (!resultado.Exito || archivo == null)
                return RedirectToPage(new { error = resultado.Mensaje });

            return File(archivo.Contenido, archivo.TipoContenido, archivo.NombreArchivo);
        }
    }
}
