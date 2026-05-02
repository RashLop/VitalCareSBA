namespace VitalCareSBA.ServicioVentas.CasosDeUso.Validadores //ProyectoArqSoft.Application.Interfaces
{
    public interface IResult<T>
    {
        Result Validar(T entidad);
    }
}
