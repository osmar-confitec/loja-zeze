using ApiGatewayUser.Services;
using BuildBlockCore.Identity;
using BuildBlockCore.Models;
using BuildBlockCore.Utils;
using BuildBlockServices.Controllers;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Request.Users;
using BuildBlockServices.Response;
using BuildBlockServices.Response.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayUser.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [Authorize]
    public class UserController : MainController
    {
        readonly IUserService _userService;
        readonly ICustomerService _customerService;
        public UserController(IUserService userService,
                              ICustomerService customerService,
                              LNotifications notifications) : base(notifications)
        {
            _userService = userService;
            _customerService = customerService;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromQuery] UserLoginRequest userLogin)
        {
            if (!ModelState.IsValid) return ReturnModelState(ModelState);

            return await ExecControllerBaseResponseApiAsync<UserLoginResponse, UserLoginDto>(() => _userService.Login(userLogin));
        }

        [HttpPost("nova-conta")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegister)
        {
            if (!ModelState.IsValid) return ReturnModelState(ModelState);
            return await ExecControllerBaseResponseApiAsync<UserRegisterResponse, UserRegisterDto>(() => _userService.Insert(userRegister));
        }

        [HttpDelete("deletar-customer/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
            => await ExecControllerBaseResponseApiAsync<UserDeleteResponse, UserDeleteDto>(() => _customerService.DeleteCustomer(id));

        [HttpGet("obter-customer")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> GetCustomer([FromQuery] UserListRequest userListRequest)
         =>   await ExecControllerBaseResponseApiPagedAsync<UserListResponse,PagedDataResponse<UserListDto>, UserListDto>(() =>   _customerService.GetUserList(userListRequest));


        [HttpPut("atualizar-customer")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UserUpdateRequest userUpdateRequest)
             => await ExecControllerBaseResponseApiAsync<UserUpdateResponse, UserUpdateDto>(() => _customerService.UpdateCustomer(userUpdateRequest));

        [HttpGet("obter-customer-id/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> GetCustomerById([FromRoute] string id)
            => await ExecControllerBaseResponseApiAsync<UserItemResponse, UserUpdateDto>(() => _customerService.GetUserList(id));
    }
}
