using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.Entidades.DTOs;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosSalida //ProyectoArqSoft.Application.Ports.Output 
{
    public interface IVentaOutputPort//IVentaRepository
    {
        IEnumerable<Venta> GetAll();
        IEnumerable<Venta> GetAll(string filtro);

        Venta? GetById(int id);
        int Count();

        List<DetalleVenta> GetDetallesByVentaId(int idVenta);

        Result RegistrarVenta(Venta venta);
        Result ActualizarVenta(Venta venta);
        Result AnularVentaLogicamente(int idVenta, int idUsuarioEditor);
        IEnumerable<ReporteVentasPorRolDto> ReporteVentasPorRol(DateTime fechaInicio, DateTime fechaFin);
    }
}
