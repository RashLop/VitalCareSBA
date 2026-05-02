using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.Validators;
using System.Data;

namespace ProyectoArqSoft.Application.Facades ////
//namespace VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas
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