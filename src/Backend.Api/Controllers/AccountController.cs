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
        public async Task<IActionResult> CreateApplicationUserAsync([FromBody] CreateAccountDto dto)
        {
            var resutl = await _mediator.Send(new CreateApplicationUserCommand(dto.PhoneNumber, dto.Password, dto.ConfirmPassword, dto.UserRole));
            return Ok(resutl);
        }

        [Authorize]
        [HttpGet("test-auth/")]
        public async Task<IActionResult> TestAuthAsync()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var phoneNumber = User.FindFirst(ClaimTypes.Name)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                AccountId = accountId,
                PhoneNumber = phoneNumber,
                Role = role
            });
        }


        [HttpGet("get-accounts/")]
        public async Task<IActionResult> GetAccountsAsync()
        {
            var result = await _mediator.Send(new GetAccountsQuery());
            return Ok(result);
        }

        [HttpGet("get-account/{id}")]
        public async Task<IActionResult> GetAccountById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetAccountByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInDto dto)
        {
            var result = await _mediator.Send(new LogInCommand(dto.PhoneNumber, dto.Password));
            return Ok(result);
        }



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
