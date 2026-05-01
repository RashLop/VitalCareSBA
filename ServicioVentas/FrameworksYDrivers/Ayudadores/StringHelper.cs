namespace ServicioVentas.FrameworksYDrivers.Ayudadores
{
    public static class StringHelper
    {
        public static string QuitarEspacios(string? valor)
        {
            return string.IsNullOrWhiteSpace(valor)
                ? string.Empty
                : valor.Replace(" ", string.Empty).Trim();
        }

        public static string LimpiarEspacios(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return string.Join(" ", valor.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
