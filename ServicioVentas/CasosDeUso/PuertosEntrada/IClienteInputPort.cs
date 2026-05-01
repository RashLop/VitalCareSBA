using ServicioVentas.AdaptadoresDeInterfaz.DTOs;

namespace ServicioVentas.CasosDeUso.PuertosEntrada
{
    public interface IClienteInputPort
    {
        object ObtenerTodos(string filtro);
        object ObtenerPorId(int id);
        object Crear(ClienteCrearActualizarDto dto);
        object Actualizar(int id, ClienteCrearActualizarDto dto);
        object Eliminar(int id, int idUsuario);
    }
}
