namespace ServicioUsuarios.Dominio.Validadores
{
    public class LoginValidador
    {
        public void Validar(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("El email es obligatorio");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("La contraseña es obligatoria");

            if (!email.Contains("@"))
                throw new Exception("Formato de email inválido");
        }
    }
}