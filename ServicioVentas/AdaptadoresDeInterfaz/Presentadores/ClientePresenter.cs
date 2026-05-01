using ServicioVentas.AdaptadoresDeInterfaz.DTOs;
using ServicioVentas.CasosDeUso.PuertosSalida;
using ServicioVentas.Entidades;

namespace ServicioVentas.AdaptadoresDeInterfaz.Presentadores
{
    public class ClientePresenter : IClienteOutputPort
    {
        public object PresentarExito<T>(string mensaje, T? datos)
        {
            return new RespuestaOperacionDto<T>
            {
                Exito = true,
                Mensaje = mensaje,
                Datos = datos
            };
        }

        public object PresentarError(string mensaje)
        {
            return new RespuestaOperacionDto<object>
            {
                Exito = false,
                Mensaje = mensaje
            };
        }

        public ClienteResponseDto PresentarCliente(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                IdCliente = cliente.IdCliente,
                FechaRegistro = cliente.FechaRegistro,
                UltimaActualizacion = cliente.UltimaActualizacion,
                IdUsuario = cliente.IdUsuario,
                Estado = cliente.Estado,
                Nit = cliente.Nit,
                RazonSocial = cliente.RazonSocial,
                CorreoElectronico = cliente.CorreoElectronico
            };
        }

        public IEnumerable<ClienteResponseDto> PresentarClientes(IEnumerable<Cliente> clientes)
        {
            return clientes.Select(PresentarCliente);
        }
    }
}
