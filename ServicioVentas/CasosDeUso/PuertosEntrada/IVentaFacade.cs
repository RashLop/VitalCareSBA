using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using VitalCareSBA.ServicioVentas.Entidades.DTOs;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada //ProyectoArqSoft.Application.Interfaces 
{
    public interface IVentaFacade
    {
        IEnumerable<Venta> ObtenerVentas(string filtro);
        Venta? ObtenerVentaPorId(int id);
        List<DetalleVenta> ObtenerDetalles(int idVenta);

        Result CrearVenta(
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detalles);

        Result ActualizarVenta(
            int idVenta,
            int idCliente,
            string metodoPago,
            List<DetalleVentaInputDto> detalles,
            int idUsuarioEditor);

        Result AnularVenta(int idVenta, int idUsuarioEditor);

        IEnumerable<Cliente> ObtenerClientes(string filtro = "");
        IEnumerable<Medicamento> ObtenerMedicamentos();

        IEnumerable<ReporteVentasPorRolDto> ReporteVentasPorRol(DateTime fechaInicio, DateTime fechaFin);
    }
}