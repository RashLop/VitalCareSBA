namespace ServicioVentas.CasosDeUso.PuertosEntrada
{
    public interface IClienteInputPort
    {
        IEnumerable<Entidades.Cliente> ObtenerTodos(string filtro);
        Entidades.Cliente? ObtenerPorId(int id);
        (bool Exito, string Mensaje) Crear(Entidades.Cliente cliente);
        (bool Exito, string Mensaje) Actualizar(int id, Entidades.Cliente cliente);
        (bool Exito, string Mensaje) Eliminar(int id, int idUsuario);
    }
}
