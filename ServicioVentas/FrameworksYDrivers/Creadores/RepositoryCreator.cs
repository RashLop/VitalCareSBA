using ServicioVentas.AdaptadoresDeInterfaz.Gateways;

namespace ServicioVentas.FrameworksYDrivers.Creadores
{
    public abstract class RepositoryCreator<T>
    {
        public abstract IRepository<T> CreateRepo();
    }
}
