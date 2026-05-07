using Microsoft.AspNetCore.Mvc;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Reportes;

namespace VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Controladores
{
    [ApiController]
    [Route("api/ventas")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaFacade ventaFacade;

        public VentaController(IVentaFacade ventaFacade)
        {
            this.ventaFacade = ventaFacade;
        }

        [HttpGet]
        public IActionResult ObtenerTodos([FromQuery] string filtro = "")
        {
            return Ok(ventaFacade.ObtenerVentas(filtro));
        }

        [HttpGet("{id:int}")]
        public IActionResult ObtenerPorId(int id)
        {
            Venta? venta = ventaFacade.ObtenerVentaPorId(id);

            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada." });

            venta.Detalles = ventaFacade.ObtenerDetalles(id);

            return Ok(venta);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Venta venta)
        {
            var resultado = ventaFacade.CrearVenta(
                venta.IdCliente,
                venta.IdUsuario,
                venta.MetodoPago,
                venta.Detalles.Select(d => new DetalleVentaInputDto
                {
                    IdMedicamento = d.IdMedicamento,
                    Cantidad = d.Cantidad
                }).ToList()
            );

            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Venta registrada correctamente." });
        }

        [HttpPut("{id:int}")]
        public IActionResult Actualizar(int id, [FromBody] Venta venta)
        {
            var resultado = ventaFacade.ActualizarVenta(
                id,
                venta.IdCliente,
                venta.MetodoPago,
                venta.Detalles.Select(d => new DetalleVentaInputDto
                {
                    IdMedicamento = d.IdMedicamento,
                    Cantidad = d.Cantidad
                }).ToList(),
                venta.IdUsuarioEditor ?? 0
            );

            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Venta actualizada correctamente." });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Anular(int id, [FromQuery] int idUsuarioEditor)
        {
            var resultado = ventaFacade.AnularVenta(id, idUsuarioEditor);

            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Venta anulada correctamente." });
        }

        [HttpGet("reporte-ventas-por-rol")]
        public IActionResult ReporteVentasPorRolMensual()
        {
            DateTime hoy = DateTime.Now;
            DateTime fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

            var reporte = ventaFacade.ReporteVentasPorRol(fechaInicio, fechaFin);

            return Ok(new
            {
                mensaje = "Reporte mensual de ventas por rol generado correctamente.",
                desde = fechaInicio.ToString("dd/MM/yyyy"),
                hasta = fechaFin.ToString("dd/MM/yyyy"),
                data = reporte
            });
        }

        [HttpGet("reporte-ventas-por-rol/pdf")]
        public IActionResult DescargarReporteVentasPorRolPdf()
        {
            DateTime hoy = DateTime.Now;
            DateTime fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

            var reporte = ventaFacade.ReporteVentasPorRol(fechaInicio, fechaFin).ToList();

            byte[] pdf = ReporteVentasPorRolPdf.Generar(fechaInicio, fechaFin, reporte);

            string nombreArchivo = $"reporte-ventas-por-rol-{hoy:yyyy-MM-dd-HHmmss}.pdf";

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return File(pdf, "application/pdf", nombreArchivo);
        }
    }
}