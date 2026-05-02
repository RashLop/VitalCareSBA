using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosSalida;
using VitalCareSBA.ServicioVentas.FrameworksYDrivers.Repositorios;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores //ProyectoArqSoft.Infrastructure.Creadores 
{
    public class VentaRepositoryCreator
    {
        public IVentaOutputPort CreateRepo() //IVentaRepository
        {
            return new VentaRepository();
        }
    }
}
