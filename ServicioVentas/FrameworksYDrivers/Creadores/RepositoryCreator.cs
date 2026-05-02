using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores
{
    public abstract class RepositoryCreator<T>
    {
        public abstract IRepository<T> CreateRepo();
    }
}
