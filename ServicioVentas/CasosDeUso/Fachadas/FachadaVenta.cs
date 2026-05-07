using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using VitalCareSBA.ServicioVentas.Entidades.DTOs;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas //ProyectoArqSoft.Application.Facades 
{
    public class FachadaVenta
    {
        private readonly IVentaInputPort _ventaService; //IVentaService

        public FachadaVenta(IVentaInputPort ventaService) //IVentaService
        {
            _ventaService = ventaService;
        }

        public Result RegistrarVenta(
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detalles)
        {
            return _ventaService.Crear(
                idCliente,
                idUsuario,
                metodoPago,
                detalles);
        }

        public Result ActualizarVenta(
            int idVenta,
            int idCliente,
            string metodoPago,
            List<DetalleVentaInputDto> detalles,
            int idUsuarioEditor)
        {
            return _ventaService.Actualizar(
                idVenta,
                idCliente,
                metodoPago,
                detalles,
                idUsuarioEditor);
        }

        public IEnumerable<Venta> ObtenerVentas(string filtro)
            => _ventaService.ObtenerTodos(filtro);

        public Venta? ObtenerVentaPorId(int id)
            => _ventaService.ObtenerPorId(id);

        public List<DetalleVenta> ObtenerDetalles(int idVenta)
            => _ventaService.ObtenerDetallesPorVenta(idVenta);

        public IEnumerable<ReporteVentasPorRolDto> ReporteVentasPorRol(DateTime fechaInicio, DateTime fechaFin)
            => _ventaService.ReporteVentasPorRol(fechaInicio, fechaFin);
    }
}