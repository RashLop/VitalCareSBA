using System.Data;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada
{
     public interface IMedicamentoInputPort
    {
        IEnumerable<Medicamento> ObtenerTodos();
        IEnumerable<Medicamento> ObtenerTodos(string filtro);
        Medicamento? ObtenerPorId(int id);
        DataTable ObtenerDestacados();

        Result Crear(Medicamento medicamento);

        Result Actualizar(int id, Medicamento medicamento);

        Result EliminarLogicamente(int id, int idUsuario);

    }
}