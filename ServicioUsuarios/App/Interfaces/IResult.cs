namespace ServicioUsuarios.Dominio.Validadores
{
    public interface IResult<T>
    {
        Result Validar(T entidad);
    }
}
