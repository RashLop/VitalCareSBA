using System.Data;
using VitalCareSBA.ServicioVentas.Entidades;

namespace VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways
{
    public interface    IMedicamentoRepository : IRepository<Medicamento>
    {
        DataTable GetDestacados();
        int Count();
        int UpdateStock(int idMedicamento, int cantidad, bool esEntrada, int idUsuario);
    }
}
