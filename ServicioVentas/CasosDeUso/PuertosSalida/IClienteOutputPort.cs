using ServicioVentas.AdaptadoresDeInterfaz.DTOs;

namespace ServicioVentas.CasosDeUso.PuertosSalida
{
    public interface IClienteOutputPort
    {
        object PresentarExito<T>(string mensaje, T? datos);
        object PresentarError(string mensaje);
        ClienteResponseDto PresentarCliente(Entidades.Cliente cliente);
        IEnumerable<ClienteResponseDto> PresentarClientes(IEnumerable<Entidades.Cliente> clientes);
    }
}
