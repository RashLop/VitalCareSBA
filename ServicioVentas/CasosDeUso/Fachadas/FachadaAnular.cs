using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas //ProyectoArqSoft.Application.Facades 
{
    public class FachadaAnular
    {
        private readonly IVentaService _ventaService;

        public FachadaAnular(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        public Result AnularVenta(
            int idVenta,
            int idUsuarioEditor)
        {
            return _ventaService.EliminarLogicamente(
                idVenta,
                idUsuarioEditor);
        }
    }
}
