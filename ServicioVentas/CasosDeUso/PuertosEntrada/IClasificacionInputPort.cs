using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada
{
    public interface IClasificacionInputPort
    {
        IEnumerable<Clasificacion> ObtenerTodos();
        IEnumerable<Clasificacion> ObtenerTodos(string filtro);

        Clasificacion? ObtenerPorId(int id);

        Result Crear(string nombre, string origen, string descripcion, int idUsuario);

        Result Actualizar(int id, string nombre, string origen, string descripcion, int idUsuario);

        Result EliminarLogicamente(int id, int idUsuario);
    }
}
