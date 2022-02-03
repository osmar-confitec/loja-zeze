using BuildBlockCore.Utils;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Models;
using BuildBlockServices.Request.Users;
using BuildBlockServices.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayUser.Services
{

    public interface ICustomerService {

        Task<UserListResponse> GetUserList(UserListRequest userLoginRequest);

        Task<UserItemResponse> GetUserList(string id);

        Task<UserDeleteResponse> DeleteCustomer(Guid Id);
        Task<UserUpdateResponse> UpdateCustomer(UserUpdateRequest userUpdateRequest);

    }
    public class CustomerService : BaseService,  ICustomerService
    {

        readonly HttpClient _httpClient;
        public CustomerService(LNotifications notification, HttpClient httpClient) : base(notification)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDeleteResponse> DeleteCustomer(Guid id)
        {
            var responseList = await _httpClient.DeleteAsync($"api/v1/customer/deletar-customer/{id}");
            await TreatErrorsResponse<UserUpdateDto>(responseList);
            if (_notification.Any())
                return null;
            return (await DeserializeObjResponse<UserDeleteResponse>(responseList));
        }

        public async Task<UserListResponse> GetUserList(UserListRequest  userLoginRequest)
        {
            var responseList =  await _httpClient.GetAsync($"api/v1/customer/obter-customer?{userLoginRequest.GetQueryString()}");
            await TreatErrorsResponse<UserListDto>(responseList);
            if (_notification.Any())
                return null;
            return (await DeserializeObjResponse<UserListResponse>(responseList));
        }

        public async Task<UserItemResponse> GetUserList(string id)
        {
            var responseList = await _httpClient.GetAsync($"api/v1/customer/obter-customer-id/{id}");
            await TreatErrorsResponse<UserUpdateDto>(responseList);
            if (_notification.Any())
                return null;
            return (await DeserializeObjResponse<UserItemResponse>(responseList));
        }

        public async Task<UserUpdateResponse> UpdateCustomer(UserUpdateRequest userUpdateRequest)
        {
            var httpContent = GetContentJsonUTF8(userUpdateRequest);
            var responseList = await _httpClient.PutAsync($"api/v1/customer/atualizar-customer", httpContent);
            await TreatErrorsResponse<UserUpdateDto>(responseList);
            if (_notification.Any())
                return null;
            return (await DeserializeObjResponse<UserUpdateResponse>(responseList));
        }
    }
}
