using BuildBlockCore.Utils;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Models;
using BuildBlockServices.Request.Users;
using BuildBlockServices.Response;
using BuildBlockServices.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiGatewayUser.Services
{

    public interface IUserService {


        Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest);

        Task<UserUpdateResponse> Update(UserUpdateRequest userLoginRequest);

        Task<UserRegisterResponse> Insert(UserRegisterRequest userLoginRequest);

    }
    public class UserService : BaseService, IUserService
    {
        readonly HttpClient _httpClient;
        public UserService(LNotifications notification, HttpClient httpClient) : base(notification)
        {
            _httpClient = httpClient;
        }

        public async Task<UserRegisterResponse> Insert(UserRegisterRequest  userLoginRequest)
        {
            var httpContent = GetContentJsonUTF8(userLoginRequest);
            var responseLogin = await _httpClient.PostAsync($"api/v1/user/nova-conta", httpContent);
            await TreatErrorsResponse<UserRegisterDto>(responseLogin);
            if (_notification.Any())
                return null;
            return (await DeserializeObjResponse<UserRegisterResponse>(responseLogin)) ;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
        {
            var responseLogin =  await _httpClient.GetAsync($"api/v1/user/login?{userLoginRequest.GetQueryString()}");
            await TreatErrorsResponse<UserLoginDto>(responseLogin);
            if (_notification.Any())
                return null;
           return (await DeserializeObjResponse<UserLoginResponse>(responseLogin));
        }

        public  Task<UserUpdateResponse> Update(UserUpdateRequest userLoginRequest)
        {
            throw new NotImplementedException();
        }
    }

}
