using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;

namespace ProyectoArqSoft.Infrastructure.Creadores ///////
//namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Creadores
{
    public class VentaRepositoryCreator
    {
        public IVentaOutputPort CreateRepo() //IVentaRepository
        {
            return new VentaRepository();
        }
    }
}
