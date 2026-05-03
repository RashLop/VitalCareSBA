using MySql.Data.MySqlClient;
using ServicioUsuarios.Dominio.Modelos;
using ServicioUsuarios.Dominio.Puertos.PuertoSalida;
using ServicioUsuarios.Infraestructura.Persistencia.Conexion;

namespace ServicioUsuarios.Infraestructura.Persistencia.Repositorios
{
    public class UsuarioTokenRepositorio : IUsuarioTokenRepositorio
    {
        private readonly ConexionStringSingleton conexion;

        public UsuarioTokenRepositorio(ConexionStringSingleton conexion)
        {
            this.conexion = conexion;
        }

        public void Guardar(UsuarioToken token)
        {
            const string query = @"INSERT INTO usuario_token
            (
                usuario_idUsuario,
                token_hash,
                tipo_token,
                fecha_creacion,
                fecha_expiracion,
                revocado,
                usado,
                fecha_uso,
                fecha_revocacion
            )
            VALUES
            (
                @usuario_idUsuario,
                @token_hash,
                @tipo_token,
                @fecha_creacion,
                @fecha_expiracion,
                @revocado,
                @usado,
                @fecha_uso,
                @fecha_revocacion
            )";

            using var conn = new MySqlConnection(conexion.CadenaConexion);
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@usuario_idUsuario", token.UsuarioIdUsuario);
            cmd.Parameters.AddWithValue("@token_hash", token.TokenHash);
            cmd.Parameters.AddWithValue("@tipo_token", token.TipoToken);
            cmd.Parameters.AddWithValue("@fecha_creacion", token.FechaCreacion);
            cmd.Parameters.AddWithValue("@fecha_expiracion", token.FechaExpiracion);
            cmd.Parameters.AddWithValue("@revocado", token.Revocado);
            cmd.Parameters.AddWithValue("@usado", token.Usado);
            cmd.Parameters.AddWithValue("@fecha_uso", token.FechaUso ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_revocacion", token.FechaRevocacion ?? (object)DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}