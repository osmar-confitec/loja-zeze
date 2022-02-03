using BuildBlockServices.Request.Users;
using BuildBlockServices.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZeZe.Services
{
    public interface IAuthService
    {

        Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest);

        Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest);


        Task<UserListResponse> GetCustomer(UserListRequest userListRequest);

        Task<UserUpdateResponse> UpdateCustomer(UserUpdateRequest  userUpdateRequest);

        Task<UserUpdateResponse> GetCustomer(string id);

        Task<UserDeleteResponse> DeleteCustomer(Guid id);

       




    }
}
