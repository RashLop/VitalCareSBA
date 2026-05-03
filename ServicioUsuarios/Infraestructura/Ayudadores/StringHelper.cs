namespace ServicioUsuarios.Infraestructura.Ayudadores
{
    public static class StringHelper
    {
        public static string LimpiarEspacios(string? texto)
        {
            return QuitarEspacios(texto);
        }

        public static string QuitarEspacios(string? texto)
        {
            return string.IsNullOrWhiteSpace(texto)
                ? string.Empty
                : texto.Trim();
        }
    }
}
