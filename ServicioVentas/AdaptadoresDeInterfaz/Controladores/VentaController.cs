using Microsoft.AspNetCore.Mvc;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.Entidades;

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
    }
}