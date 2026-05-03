namespace ServicioUsuarios.CasosDeUso.Validadores
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;

        private Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static Result Ok(string message = "") => new Result(true, message);

        public static Result Fail(string message) => new Result(false, message);
    }
}
