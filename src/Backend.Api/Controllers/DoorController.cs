namespace Backend.Api.Controllers
{
    [Route("api/door")]
    [ApiController]
    public class DoorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add/")]
        public async Task<IActionResult> AddDoorAsync([FromBody] AddDoorDto dto)
        {
            var result = await _mediator.Send(new CreateDoorCommand(dto.FacilityId, dto.Name, dto.Type, dto.RoomNumber));
            return Ok(result);
        }

        [HttpPost("add-range/")]
        public async Task<IActionResult> AddDoorsAsync([FromBody] AddDoorsDto dto )
        {
            var result = await _mediator.Send(new AddDoorsCommand(dto.FacilityId, dto.Type, dto.RoomNumberFrom, dto.RoomNumberTo));
            return Ok(result);
        }


        [HttpGet("all/")]
        public async Task<IActionResult> GetDoorsAsync()
        {
            var result = await _mediator.Send(new GetDoorsQuery());
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDoorAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteDoorCommand(id));
            return Ok("deleted");
        }






    }
}
