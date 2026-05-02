using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using System.Data;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosSalida //ProyectoArqSoft.Application.Ports.Output 
{
    public interface IVentaOutputPort//IVentaRepository
    {
        DataTable GetAll();
        DataTable GetAll(string filtro);

        Venta? GetById(int id);
        int Count();

        List<DetalleVenta> GetDetallesByVentaId(int idVenta);

        Result RegistrarVenta(Venta venta);
        Result ActualizarVenta(Venta venta);
        Result AnularVentaLogicamente(int idVenta, int idUsuarioEditor);
    }
}
