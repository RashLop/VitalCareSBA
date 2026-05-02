using System.Data;
using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.CasosDeUso.Utilidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using VitalCareSBA.ServicioVentas.Entidades;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.Interactores
{
    public class MedicamentoService : IMedicamentoInputPort
    {
        private readonly IMedicamentoRepository _repository;
        private readonly IResult<Medicamento> _validador;


        public MedicamentoService(
            IMedicamentoRepository repository,
            IResult<Medicamento> validadorn)
        {
            _repository = repository;
            _validador = validadorn;
        }

        public IEnumerable<Medicamento> ObtenerTodos()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Medicamento> ObtenerTodos(string filtro)
        {
            return _repository.GetAll(filtro);
        }

        public Medicamento? ObtenerPorId(int id)
        {
            return _repository.GetById(id);
        }

        public DataTable ObtenerDestacados()
        {
            return _repository.GetDestacados();
        }

        public Result Crear(Medicamento medicamento)
        {
            var validacion = _validador.Validar(medicamento);
            if (validacion.IsSuccess == false)
                return validacion;
            if (_repository.Insert(medicamento) <= 0)
                return Result.Fail("No se pudo registrar el medicamento.");
            return Result.Ok();
        }   

        public Result Actualizar(int id, Medicamento medicamento)
        {
            var validacion = _validador.Validar(medicamento);
            if (validacion.IsSuccess == false)
                return validacion;
            if (medicamento.Id == null || _repository.GetById(medicamento.Id.Value) == null)
                return Result.Fail("El medicamento no existe.");

            if (_repository.Update(medicamento) <= 0)
                return Result.Fail("No se pudo actualizar el medicamento.");

            return Result.Ok();
        }

        public Result EliminarLogicamente(int id, int idUsuario)
        {
            Medicamento medicamento = new Medicamento
            {
                Id = id,
                IdUsuario = idUsuario
            };

            if (_repository.Delete(medicamento) <= 0)
                return Result.Fail("No se pudo eliminar el medicamento.");

            return Result.Ok();
        }

        private static void LimpiarCampos(Medicamento medicamento)
        {
            medicamento.Nombre = StringHelper.LimpiarEspacios(medicamento.Nombre);
            medicamento.Presentacion = StringHelper.LimpiarEspacios(medicamento.Presentacion);
            medicamento.Concentracion = StringHelper.LimpiarEspacios(medicamento.Concentracion);
        }

    }
}