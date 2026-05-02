using VitalCareSBA.ServicioVentas.Entidades;
using VitalCareSBA.ServicioVentas.CasosDeUso.Validadores;
using System.Data;

namespace VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada //ProyectoArqSoft.Application.Interfaces 
{
    public interface IVentaInputPort //IVentaService.cs
    {
        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);

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
    }
}
