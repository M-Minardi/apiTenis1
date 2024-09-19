
using apiTenis.Interfaces;
using apiTenis.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiTenis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly IJugadorBusiness _jugadorBusiness;
        public JugadorController(IJugadorBusiness jugadorBusiness) {
            _jugadorBusiness = jugadorBusiness;   
        }

        [HttpGet]
        public async Task<ActionResult<List<JugadorDTO>>> Get()
        {            
            var result = await _jugadorBusiness.Get();
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet("paginar")]
        public async Task<ActionResult<List<JugadorDTO>>> GetPaginar([FromQuery] PaginacionDTO paginacionDTO){          
            var result = await _jugadorBusiness.GetPaginacion(paginacionDTO);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}", Name="getJugador")]
        public async Task<ActionResult<JugadorDTO>> Get(int id)
        {
            var result = await _jugadorBusiness.Get(id);
            if(result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet("filtro", Name = "getJugadorFiltro")]
        public async Task<ActionResult<List<JugadorDTO>>> GetFiltro([FromQuery] JugadorFiltroDTO dto)
        {
            var result = await _jugadorBusiness.GetFiltro(dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] JugadorDTO dto)
        {
            if (dto == null) return BadRequest();
            var result = await _jugadorBusiness.Create(dto);
            return CreatedAtRoute("getJugador", new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] JugadorDTO dto)
        {
            if(id != dto.Id) return BadRequest();
            var result = await _jugadorBusiness.Update(id, dto);
            if(!result) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _jugadorBusiness.Delete(id);
            if(!result) return NotFound();
            return NoContent();
        }
    }
}
