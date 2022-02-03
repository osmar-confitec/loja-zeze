using BuildBlockServices.Request.Users;
using BuildBlockServices.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebZeZe.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest usuarioLogin)
        => await GetAsync<UserLoginResponse, UserLoginRequest>(usuarioLogin, "api/v1/user/login");

        public async Task<UserRegisterResponse> Register(UserRegisterRequest usuarioRegistro)
        => await PostAsync<UserRegisterResponse, UserRegisterRequest>(usuarioRegistro, "api/v1/user/nova-conta");
       
        public async Task<UserListResponse> GetCustomer( UserListRequest userListRequest)
        => await GetAsync<UserListResponse, UserListRequest>(userListRequest, "api/v1/user/obter-customer");

        public async Task<UserUpdateResponse> GetCustomer(string id)
        => await GetAsync<UserUpdateResponse, string>(null, $"api/v1/user/obter-customer-id/{id}");

        public async Task<UserUpdateResponse> UpdateCustomer(UserUpdateRequest userUpdateRequest)
         => await PutAsync<UserUpdateResponse, UserUpdateRequest>(userUpdateRequest, $"api/v1/user/atualizar-customer");

        public async Task<UserDeleteResponse> DeleteCustomer(Guid id)
        => await DeleteAsync<UserDeleteResponse, string>(null, $"api/v1/user/deletar-customer/{id}");
    }
}
