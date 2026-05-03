namespace ServicioUsuarios.Infraestructura.Persistencia.Conexion
{
    public class ConexionStringSingleton
    {
        public string CadenaConexion { get; }

        public ConexionStringSingleton(IConfiguration config)
        {
            CadenaConexion = config.GetConnectionString("MySqlConnection")
                ?? throw new Exception("No se encontró la cadena de conexión");
        }
    }
}