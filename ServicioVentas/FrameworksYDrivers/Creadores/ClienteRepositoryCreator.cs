using ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using ServicioVentas.Entidades;
using ServicioVentas.FrameworksYDrivers.Repositorios;

namespace ServicioVentas.FrameworksYDrivers.Creadores
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
