namespace Backend.Api.Controllers
{
    [Route("api/facility")]
    [ApiController]
    public class FacilityController : ControllerBase
    {

        private readonly IMediator _mediator;

        public FacilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add/")]
        public async Task<IActionResult> AddFacilityAsync([FromBody] AddFacilityDto dto )
        {
            var result = await _mediator.Send(new CreateFacilityCommand(dto.Name, dto.Type));
            return Ok(result);
        }

        [HttpGet("all/")]
        public async Task<IActionResult> GetAllFacilityAsync()
        {
            var result = await _mediator.Send(new GetFacilitiesQuery());
            return Ok(result);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetFacilityByIdAsync([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id must be a Guid");
            var result = await _mediator.Send(new GetFacilityByIdQuery(id));
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFacilityAsync([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id must be a Guid");
            await _mediator.Send(new DeleteFacilityCommand(id));
            return Ok("deleted");
        }


        // 1- update facility



    }
}
