using MediatR;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Dtos;
using Backend.Application.Features.Accounts;
using Backend.Application.Features;

namespace Backend.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-account/")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountDto dto)
        {
            var resutl = await _mediator.Send(new CreateUserCommand(dto.PhoneNumber, dto.Password, dto.Email, dto.UserRole));
            return Ok(resutl);
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInDto dto)
        {
            var result = await _mediator.Send(new LogInCommand(dto.PhoneNumber, dto.Password));
            return Ok(result);
        }

        //[HttpGet("get-account/")]
        //public async Task<IActionResult> GetAccountAsync([FromBody] GetAccountDto dto)

        //[HttpPost("check-username-existence")]

        [HttpPost("phone-number-code-request/")]
        public async Task<IActionResult> SendVerifyPhoneNumberCodeAsync([FromBody] SendVerifyPhoneNumberCodeDto dto)
        {
            bool result = await _mediator.Send(new VerifyPhoneNumberRequestCommand(dto.PhoneNumber));
            return Ok("Sended Successfully");
        }

        [HttpPost("verify-phone-number-code/")]
        public async Task<IActionResult> VerifyPhoneNumberCodeAsync([FromBody] VerifyPhoneNumberDto dto)
        {
            string result = await _mediator.Send(new VerifyPhoneNumberCommand(dto.PhoneNumber, dto.Code));
            return Ok(result);
        }


        [HttpGet("create-driver-role")]
        public async Task<IActionResult> CreateRoleForTesting()
        {
            var result = await _mediator.Send(new CreateRoleForTestingCommand());
            return Ok(result);
        }

        //[Authorize]
        //[HttpPost("face-verification")]
        //public async Task<IActionResult> FaceVerificationAsync([FromForm] FaceVerificationDto dto)
        //{

        //}


    }
}
