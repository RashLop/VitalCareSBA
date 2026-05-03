using MySql.Data.MySqlClient;
using ServicioUsuarios.Dominio.Modelos;
using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Persistencia.Conexion;

namespace ServicioUsuarios.Infraestructura.Persistencia.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ConexionStringSingleton conexion;

        public UsuarioRepositorio(ConexionStringSingleton conexion)
        {
            this.conexion = conexion;
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            const string query = @"SELECT id, email, user_name, password_hash, role, activo, must_change_password
                                  FROM usuario
                                  WHERE email = @email
                                  LIMIT 1";

            using var conn = new MySqlConnection(conexion.CadenaConexion);
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@email", email);

            conn.Open();
            var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new Usuario
            {
                IdUsuario = Convert.ToInt32(reader["id"]),
                Email = reader["email"].ToString() ?? "",
                UserName = reader["user_name"].ToString() ?? "",
                PasswordHash = reader["password_hash"].ToString() ?? "",
                Role = reader["role"].ToString() ?? "",
                Activo = Convert.ToSByte(reader["activo"]),
                MustChangePassword = Convert.ToSByte(reader["must_change_password"])
            };
        }
    }
}