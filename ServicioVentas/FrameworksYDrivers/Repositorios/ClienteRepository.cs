using MySql.Data.MySqlClient;
using ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using ServicioVentas.Entidades;
using ServicioVentas.FrameworksYDrivers.Data;

namespace ServicioVentas.FrameworksYDrivers.Repositorios
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string connectionString;

        public ClienteRepository(ConexionString conexionString)
        {
            connectionString = conexionString.CadenaConexion;
        }

        public int Insert(Cliente cliente)
        {
            string query = @"INSERT INTO cliente
                            (nit, razon_social, correo_electronico, id_usuario, estado)
                            VALUES
                            (@nit, @razon_social, @correo_electronico, @id_usuario, @estado)";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@nit", cliente.Nit);
            command.Parameters.AddWithValue("@razon_social", cliente.RazonSocial);
            command.Parameters.AddWithValue("@id_usuario", cliente.IdUsuario);
            command.Parameters.AddWithValue("@estado", cliente.Estado);
            command.Parameters.AddWithValue(
                "@correo_electronico",
                string.IsNullOrWhiteSpace(cliente.CorreoElectronico) ? DBNull.Value : cliente.CorreoElectronico);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int Update(Cliente cliente)
        {
            string query = @"UPDATE cliente
                             SET nit = @nit,
                                 razon_social = @razon_social,
                                 correo_electronico = @correo_electronico,
                                 id_usuario = @id_usuario,
                                 ultima_actualizacion = NOW()
                             WHERE id = @id
                               AND estado = 1";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", cliente.IdCliente);
            command.Parameters.AddWithValue("@nit", cliente.Nit);
            command.Parameters.AddWithValue("@razon_social", cliente.RazonSocial);
            command.Parameters.AddWithValue("@id_usuario", cliente.IdUsuario);
            command.Parameters.AddWithValue(
                "@correo_electronico",
                string.IsNullOrWhiteSpace(cliente.CorreoElectronico) ? DBNull.Value : cliente.CorreoElectronico);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int Delete(Cliente cliente)
        {
            string query = @"UPDATE cliente
                             SET estado = 0,
                                 id_usuario = @id_usuario,
                                 ultima_actualizacion = NOW()
                             WHERE id = @id
                               AND estado = 1";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", cliente.IdCliente);
            command.Parameters.AddWithValue("@id_usuario", cliente.IdUsuario);

            connection.Open();
            return command.ExecuteNonQuery();
        }


        public IEnumerable<Cliente> GetAll()
        {
            return GetAll(string.Empty);
        }
        public IEnumerable<Cliente> GetAll(string filtro)
        {
            List<Cliente> clientes = new List<Cliente>();

            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = ConstruirQuery(filtro);
            using MySqlCommand command = new MySqlCommand(query, connection);

            AgregarParametroFiltro(command, filtro);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                clientes.Add(MapearCliente(reader));
            }

            return clientes;
        }

        public Cliente? GetById(int id)
        {
            string query = @"SELECT id, fecha_registro, ultima_actualizacion, nit, razon_social, correo_electronico, id_usuario, estado
                             FROM cliente
                             WHERE id = @id
                               AND estado = 1";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            using MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            connection.Open();

            using MySqlDataReader reader = command.ExecuteReader();
            return reader.Read() ? MapearCliente(reader) : null;
        }


        public int Count()
        {
            string query = "SELECT COUNT(*) FROM cliente WHERE estado = 1";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            using MySqlCommand command = new MySqlCommand(query, connection);

            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar());
        }
        private static string ConstruirQuery(string filtro)
        {
            string query = @"SELECT id, fecha_registro, ultima_actualizacion, nit, razon_social, correo_electronico, id_usuario, estado
                             FROM cliente
                             WHERE estado = 1";

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query += @" AND (
                                REPLACE(nit, ' ', '') LIKE @filtro
                                OR REPLACE(razon_social, ' ', '') LIKE @filtro
                                OR REPLACE(correo_electronico, ' ', '') LIKE @filtro
                            )";
            }

            query += " ORDER BY razon_social";
            return query;
        }

        private static void AgregarParametroFiltro(MySqlCommand command, string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return;

            command.Parameters.AddWithValue("@filtro", $"%{QuitarEspacios(filtro)}%");
        }

        private static Cliente MapearCliente(MySqlDataReader reader)
        {
            return new Cliente
            {
                IdCliente = Convert.ToInt32(reader["id"]),
                FechaRegistro = Convert.ToDateTime(reader["fecha_registro"]),
                UltimaActualizacion = reader["ultima_actualizacion"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["ultima_actualizacion"]),
                IdUsuario = reader["id_usuario"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["id_usuario"]),
                Estado = Convert.ToInt16(reader["estado"]),
                Nit = LimpiarEspacios(reader["nit"].ToString()),
                RazonSocial = LimpiarEspacios(reader["razon_social"].ToString()),
                CorreoElectronico = LimpiarEspacios(reader["correo_electronico"].ToString())
            };
        }

        private static string QuitarEspacios(string? valor)
        {
            return string.IsNullOrWhiteSpace(valor)
                ? string.Empty
                : valor.Replace(" ", string.Empty).Trim();
        }

        private static string LimpiarEspacios(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return string.Join(" ", valor.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}


