using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Repositorios;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores
{
    public class ClienteRepositoryCreator : RepositoryCreator<Cliente>
    {
        public override IRepository<Cliente> CreateRepo()
        {
            return new ClienteRepository();
        }

        public IClienteRepository CreateClienteRepo()
        {
            return new ClienteRepository();
        }
    }
}
