using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.Entidades.DTOs;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada //ProyectoArqSoft.Application.Interfaces 
{
    public interface IVentaInputPort //IVentaService.cs
    {
        IEnumerable<Venta> ObtenerTodos();
        IEnumerable<Venta> ObtenerTodos(string filtro);

        Venta? ObtenerPorId(int id);
        List<DetalleVenta> ObtenerDetallesPorVenta(int idVenta);

        Result Crear(
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detallesInput);

        Result Actualizar(
            int idVenta,
            int idCliente,
            string metodoPago,
            List<DetalleVentaInputDto> detallesInput,
            int idUsuarioEditor);

        Result EliminarLogicamente(int idVenta, int idUsuarioEditor);
        IEnumerable<ReporteVentasPorRolDto> ReporteVentasPorRol(DateTime fechaInicio, DateTime fechaFin);
    }
}
