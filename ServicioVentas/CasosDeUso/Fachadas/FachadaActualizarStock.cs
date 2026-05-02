using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using System.Data;
using VitalCareSBA.ServicioVentas.Entidades;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.Fachadas //ProyectoArqSoft.Application.Facades 
{
    public class FachadaActualizarStock
    {
        private readonly IMedicamentoInputPort _medicamentoService;

        public FachadaActualizarStock(
            IMedicamentoInputPort medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        public IEnumerable<Medicamento> ObtenerMedicamentos()
        {
            return _medicamentoService.ObtenerTodos();
        }
    }
}