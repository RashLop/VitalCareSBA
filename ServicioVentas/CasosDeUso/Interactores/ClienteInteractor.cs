using ServicioVentas.AdaptadoresDeInterfaz.DTOs;
using ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using ServicioVentas.CasosDeUso.PuertosEntrada;
using ServicioVentas.CasosDeUso.PuertosSalida;
using ServicioVentas.CasosDeUso.Validadores;
using ServicioVentas.Entidades;

namespace ServicioVentas.CasosDeUso.Interactores
{
    public class ClienteInteractor : IClienteInputPort
    {
        private readonly IClienteRepository clienteRepository;
        private readonly IResult<Cliente> clienteValidacion;
        private readonly IClienteOutputPort clienteOutputPort;

        public ClienteInteractor(
            IClienteRepository clienteRepository,
            IResult<Cliente> clienteValidacion,
            IClienteOutputPort clienteOutputPort)
        {
            this.clienteRepository = clienteRepository;
            this.clienteValidacion = clienteValidacion;
            this.clienteOutputPort = clienteOutputPort;
        }

        public object ObtenerTodos(string filtro)
        {
            IEnumerable<Cliente> clientes = clienteRepository.GetAll(filtro);
            return clienteOutputPort.PresentarExito(
                "Clientes obtenidos correctamente.",
                clienteOutputPort.PresentarClientes(clientes));
        }

        public object ObtenerPorId(int id)
        {
            Cliente? cliente = clienteRepository.GetById(id);
            if (cliente == null)
                return clienteOutputPort.PresentarError("Cliente no encontrado.");

            return clienteOutputPort.PresentarExito(
                "Cliente obtenido correctamente.",
                clienteOutputPort.PresentarCliente(cliente));
        }

        public object Crear(ClienteCrearActualizarDto dto)
        {
            Cliente cliente = ConstruirCliente(0, dto);

            Result validacion = ValidarCliente(cliente);
            if (!validacion.IsSuccess)
                return clienteOutputPort.PresentarError(validacion.Error);

            if (clienteRepository.Insert(cliente) <= 0)
                return clienteOutputPort.PresentarError("No se pudo registrar el cliente.");

            return clienteOutputPort.PresentarExito<object>("Cliente registrado correctamente.", null);
        }

        public object Actualizar(int id, ClienteCrearActualizarDto dto)
        {
            Cliente cliente = ConstruirCliente(id, dto);

            Result validacion = ValidarCliente(cliente);
            if (!validacion.IsSuccess)
                return clienteOutputPort.PresentarError(validacion.Error);

            if (clienteRepository.Update(cliente) <= 0)
                return clienteOutputPort.PresentarError("No se pudo actualizar el cliente.");

            return clienteOutputPort.PresentarExito<object>("Cliente actualizado correctamente.", null);
        }

        public object Eliminar(int id, int idUsuario)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                IdUsuario = idUsuario
            };

            if (clienteRepository.Delete(cliente) <= 0)
                return clienteOutputPort.PresentarError("No se pudo eliminar el cliente.");

            return clienteOutputPort.PresentarExito<object>("Cliente eliminado correctamente.", null);
        }

        private Result ValidarCliente(Cliente cliente)
        {
            Result validacion = clienteValidacion.Validar(cliente);
            if (!validacion.IsSuccess)
                return validacion;

            LimpiarCampos(cliente);
            return ValidarDuplicado(cliente);
        }

        private Result ValidarDuplicado(Cliente cliente)
        {
            if (cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase))
                return Result.Ok();

            IEnumerable<Cliente> clientes = clienteRepository.GetAll(cliente.Nit);

            foreach (Cliente clienteExistente in clientes)
            {
                string nit = QuitarEspacios(clienteExistente.Nit);

                if (nit.Equals(cliente.Nit, StringComparison.OrdinalIgnoreCase) &&
                    clienteExistente.IdCliente != cliente.IdCliente)
                {
                    return Result.Fail("Ya existe un cliente con ese NIT.");
                }
            }

            return Result.Ok();
        }

        private static Cliente ConstruirCliente(int id, ClienteCrearActualizarDto dto)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Nit = dto.Nit,
                RazonSocial = dto.RazonSocial,
                CorreoElectronico = dto.CorreoElectronico ?? string.Empty,
                IdUsuario = dto.IdUsuario
            };

            AplicarConsumidorFinal(cliente, dto.EsConsumidorFinal);
            return cliente;
        }

        private static void AplicarConsumidorFinal(Cliente cliente, bool esConsumidorFinal)
        {
            if (!esConsumidorFinal)
                return;

            cliente.Nit = "CF";
            cliente.RazonSocial = "Consumidor Final";
            cliente.CorreoElectronico = string.Empty;
        }

        private static void LimpiarCampos(Cliente cliente)
        {
            cliente.Nit = QuitarEspacios(cliente.Nit);
            cliente.RazonSocial = LimpiarEspacios(cliente.RazonSocial);
            cliente.CorreoElectronico = QuitarEspacios(cliente.CorreoElectronico);
        }

        private static string QuitarEspacios(string? valor)
        {
            return string.IsNullOrWhiteSpace(valor)
                ? string.Empty
                : valor.Replace(" ", string.Empty).Trim();
        }

        private static string LimpiarEspacios(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return string.Join(" ", valor.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
