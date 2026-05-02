using VitalCareSBA.ServicioVentas.Entidades;

namespace VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        int Count();
    }
}

