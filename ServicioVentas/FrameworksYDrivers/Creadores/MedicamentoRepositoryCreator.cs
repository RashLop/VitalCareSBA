using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Repositorios;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores
{
    public class MedicamentoRepositoryCreator : RepositoryCreator<Medicamento>
    {
        public override IRepository<Medicamento> CreateRepo()
        {
            return new MedicamentoRepository();
        }

        public IMedicamentoRepository CreateMedicamentoRepo()
        {
            return new MedicamentoRepository();
        }
    }
}