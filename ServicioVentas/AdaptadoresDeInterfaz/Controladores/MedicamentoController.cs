using Microsoft.AspNetCore.Mvc;
using VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Gateways;
using VitalCareSBA.ServicioVentas.CasosDeUso.PuertosEntrada;
using VitalCareSBA.ServicioVentas.Entidades;
namespace VitalCareSBA.ServicioVentas.AdaptadoresDeInterfaz.Controladores
{
    [ApiController]
    [Route("api/medicamentos")]
    public class MedicamentoController : ControllerBase
    {
        private readonly IMedicamentoInputPort _medicamentoInputPort;
        public MedicamentoController(IMedicamentoInputPort medicamentoInputPort) => _medicamentoInputPort = medicamentoInputPort;

        [HttpGet]
        public IActionResult ObtenerTodos([FromQuery] string filtro = "")
        {
            return Ok(_medicamentoInputPort.ObtenerTodos(filtro));
        }

        [HttpGet("{id:int}")]
        public IActionResult ObtenerPorId(int id)
        {
            Medicamento? medicamento = _medicamentoInputPort.ObtenerPorId(id);
            return medicamento == null ? NotFound(new { mensaje = "Medicamento no encontrado." }) : Ok(medicamento);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Medicamento medicamento)
        {
            var resultado = _medicamentoInputPort.Crear(medicamento);
            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error});

            return Ok(new { mensaje = "Medicamento creado correctamente." });
        }

        [HttpPut("{id:int}")]
        public IActionResult Actualizar(int id, [FromBody] Medicamento medicamento  )
        {
            var resultado = _medicamentoInputPort.Actualizar(id, medicamento);
            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error });

            return Ok(new { mensaje = "Medicamento actualizado correctamente." });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Eliminar(int id, [FromQuery] int idUsuario)
        {
            var resultado = _medicamentoInputPort.EliminarLogicamente(id, idUsuario);
            if (!resultado.IsSuccess)
                return BadRequest(new { mensaje = resultado.Error    });

            return Ok(new { mensaje = "Medicamento eliminado correctamente." });
        }
    }
}
