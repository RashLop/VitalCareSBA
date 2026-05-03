using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas //ProyectoArqSoft.Application.Facades 
{
    public class VentaFacade : IVentaFacade
    {
        private readonly FachadaVenta _fv;
        private readonly FachadaAnular _fa;
        private readonly FachadaActualizarStock _fasm;
        private readonly IClienteInputPort _clienteInputPort; //IClienteService _clienteService;

        public VentaFacade(
            FachadaVenta fv,
            FachadaAnular fa,
            FachadaActualizarStock fasm,
            IClienteInputPort clienteService)
        {
            _fv = fv;
            _fa = fa;
            _fasm = fasm;
            _clienteInputPort = clienteService;
        }

        public IEnumerable<Venta> ObtenerVentas(string filtro)
            => _fv.ObtenerVentas(filtro);

        public Venta? ObtenerVentaPorId(int id)
            => _fv.ObtenerVentaPorId(id);

        public List<DetalleVenta> ObtenerDetalles(int idVenta)
            => _fv.ObtenerDetalles(idVenta);

        public Result CrearVenta(
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detalles)
            => _fv.RegistrarVenta(
                idCliente,
                idUsuario,
                metodoPago,
                detalles);

        public Result ActualizarVenta(
            int idVenta,
            int idCliente,
            string metodoPago,
            List<DetalleVentaInputDto> detalles,
            int idUsuarioEditor)
            => _fv.ActualizarVenta(
                idVenta,
                idCliente,
                metodoPago,
                detalles,
                idUsuarioEditor);

        public Result AnularVenta(
            int idVenta,
            int idUsuarioEditor)
            => _fa.AnularVenta(
                idVenta,
                idUsuarioEditor);

        public IEnumerable<Cliente> ObtenerClientes(string filtro = "")
            => _clienteInputPort.ObtenerTodos(filtro);

        public IEnumerable<Medicamento> ObtenerMedicamentos()
            => _fasm.ObtenerMedicamentos();
    }
}
