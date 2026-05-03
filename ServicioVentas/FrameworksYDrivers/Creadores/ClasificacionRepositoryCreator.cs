using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Repositorios;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores
{
    public class ClasificacionRepositoryCreator : RepositoryCreator<Clasificacion>
    {
        public override IRepository<Clasificacion> CreateRepo()
        {
            return new ClasificacionRepository();
        }

        public IClasificacionRepository CreateClasificacionRepo()
        {
            return new ClasificacionRepository();
        }
    }
}
