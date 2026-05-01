namespace ServicioVentas.FrameworksYDrivers.Data
{
    public class ConexionString
    {
        public string CadenaConexion { get; }

        public ConexionString(IConfiguration configuration)
        {
            CadenaConexion = configuration.GetConnectionString("MySqlConnection")
                ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'MySqlConnection'.");
        }
    }
}
