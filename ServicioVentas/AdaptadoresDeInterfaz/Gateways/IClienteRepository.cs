using ServicioVentas.Entidades;

namespace ServicioVentas.AdaptadoresDeInterfaz.Gateways
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        int Count();
    }
}

