using VitalCareSBA.ServicioVentas.Entidades;

namespace VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways
{
    public interface IClasificacionRepository : IRepository<Clasificacion>
    {
        bool TieneMedicamentosActivosAsociados(int idClasificacion);
        bool ExisteNombreActivo(string nombre);
        bool ExisteNombreActivoExcluyendoId(int idClasificacion, string nombre);
    }
}
