
using apiTenis.Interfaces;
using apiTenis.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiTenis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorneoController : ControllerBase
    {
        private readonly ITorneoBusiness _torneoBusiness;
        public TorneoController(ITorneoBusiness torneoBusiness) {
            _torneoBusiness = torneoBusiness;   
        }

        [HttpGet]
        public async Task<ActionResult<List<TorneoDTO>>> Get()
        {            
            var result = await _torneoBusiness.Get();
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet("filtro", Name = "getTorneoFiltro")]
        public async Task<ActionResult<List<TorneoDTO>>> GetFiltro([FromQuery] TorneoFiltroDTO dto)
        {
            var result = await _torneoBusiness.GetFiltro(dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TorneoCrearDTO dto)
        {
            if (dto == null) return BadRequest();
            var result = await _torneoBusiness.CrearTorneoConSimulacion(dto);
            return Ok(result);
        }
    }
}
