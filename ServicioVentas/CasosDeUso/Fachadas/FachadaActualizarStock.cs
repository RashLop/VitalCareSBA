using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using System.Data;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas //ProyectoArqSoft.Application.Facades 
{
    public class FachadaActualizarStock
    {
        private readonly IMedicamentoService _medicamentoService;

        public FachadaActualizarStock(
            IMedicamentoService medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        public DataTable ObtenerMedicamentos()
        {
            return _medicamentoService.ObtenerTodos();
        }

        public Result ActualizarStock(
            int idMedicamento,
            int cantidad,
            bool esEntrada,
            int idUsuario)
        {
            return _medicamentoService.UpdateStock(
                idMedicamento,
                cantidad,
                esEntrada,
                idUsuario
            );
        }
    }
}