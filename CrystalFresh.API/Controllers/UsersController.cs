using Main.Application.Requests.Users;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;

namespace SwinvaMainV2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController(IMediator mediator, ILogger<UsersController> logger) : CustomControllerBase(mediator, logger)
    {

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(TokenModel))]
        public async Task<IActionResult> CreatedUser([FromBody] CreateUserRequest request)
        {
            request.Role= Role.User;
           return await SendRequestAsync(request);
        }

        [HttpPost("create-admin")]
        [ProducesResponseType(200, Type = typeof(TokenModel))]
        [HasPermission(Permission.SuperAdministrator)]
        public async Task<IActionResult> CreatedAdmin([FromBody] CreateUserRequest request)
        {
            request.Role = Role.Admin;
            return await SendRequestAsync(request);
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(TokenModel))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) =>
            await SendRequestAsync(request);

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request) => await SendRequestAsync(request);

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request) => await SendRequestAsync(request);


        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoveryPasswordRequest request) => await SendRequestAsync(request);

        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest request) => await SendRequestAsync(request);

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request) => await SendRequestAsync(request);

        [Authorize]
        [HttpGet("user")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        public async Task<IActionResult> GetById() => await SendRequestAsync(new UserByIdRequest());

        [Authorize]
        [HttpPut("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request) => await SendRequestAsync(request);


        [HasPermission(Permission.Administrator)]
        [HttpGet("users/{page}/{pageSize}")]
        [ProducesResponseType(200, Type = typeof(List<UserResponse>))]
        public async Task<IActionResult> AllUsers(int page, int pageSize) =>
           await SendRequestAsync(new FetchUsersRequest { Page = page, PageSize = pageSize });

        [HasPermission(Permission.Administrator)]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(Guid id) =>
           await SendRequestAsync(new UserByIdRequest(), id);

        [HasPermission(Permission.Administrator)]
        [HttpGet("user/by-email/{email}")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        public async Task<IActionResult> GetUser(string email) =>
            await SendRequestAsync(new UserByEmailRequest { Email = email });
    }
}