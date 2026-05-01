using Microsoft.AspNetCore.Mvc;
using ServicioVentas.AdaptadoresDeInterfaz.DTOs;
using ServicioVentas.CasosDeUso.PuertosEntrada;

namespace ServicioVentas.AdaptadoresDeInterfaz.Controladores
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteInputPort clienteInputPort;

        public ClienteController(IClienteInputPort clienteInputPort)
        {
            this.clienteInputPort = clienteInputPort;
        }

        [HttpGet]
        public IActionResult ObtenerTodos([FromQuery] string filtro = "")
        {
            return Ok(clienteInputPort.ObtenerTodos(filtro));
        }

        [HttpGet("{id:int}")]
        public IActionResult ObtenerPorId(int id)
        {
            object respuesta = clienteInputPort.ObtenerPorId(id);
            return ResolverRespuesta(respuesta);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] ClienteCrearActualizarDto dto)
        {
            object respuesta = clienteInputPort.Crear(dto);
            return ResolverRespuesta(respuesta);
        }

        [HttpPut("{id:int}")]
        public IActionResult Actualizar(int id, [FromBody] ClienteCrearActualizarDto dto)
        {
            object respuesta = clienteInputPort.Actualizar(id, dto);
            return ResolverRespuesta(respuesta);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Eliminar(int id, [FromQuery] int idUsuario)
        {
            object respuesta = clienteInputPort.Eliminar(id, idUsuario);
            return ResolverRespuesta(respuesta);
        }

        private IActionResult ResolverRespuesta(object respuesta)
        {
            if (respuesta is RespuestaOperacionDto<object> operacion && !operacion.Exito)
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
