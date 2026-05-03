using Microsoft.Extensions.Configuration;

namespace ServicioUsuarios.Infraestructura.Persistencia.Conexion
{
    public class ConexionStringSingleton
    {
        private static ConexionStringSingleton? instancia;
        private static readonly object bloqueo = new object();
        private readonly string cadenaConexion;

        public static ConexionStringSingleton Instancia
        {
            get
            {
                if (instancia == null)
                {
                    lock (bloqueo)
                    {
                        if (instancia == null)
                        {
                            instancia = new ConexionStringSingleton();
                        }
                    }
                }

                return instancia;
            }
        }

        private ConexionStringSingleton()
        {
            IConfigurationRoot configuracion = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            cadenaConexion = configuracion.GetConnectionString("MySqlConnection")
                ?? throw new Exception("No se encontro la cadena de conexion 'MySqlConnection'.");
        }

        public string CadenaConexion
        {
            get { return cadenaConexion; }
        }
    }
}
