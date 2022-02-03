using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebZeZe.Middleware;

namespace WebZeZe.Services
{
    public enum TypeAuthentication
    {

        Basic = 1,
        Bearear = 2

    }

    public class Authentication
    {

        public TypeAuthentication? TypeAuthentication { get; private set; }

        public string Token { get; private set; }

        public void SetBearer(string token)
        {
            TypeAuthentication = Services.TypeAuthentication.Bearear;
            Token = token;
        }
        public Authentication()
        {

        }

    }
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;

        public Authentication Authentication { get; protected set; }
        protected BaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected void SetAuthentication()
        {
            if (Authentication != null)
            {
                switch (Authentication.TypeAuthentication)
                {
                    case TypeAuthentication.Basic:
                        break;
                    case TypeAuthentication.Bearear:
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Authentication.Token);
                        break;
                    default:
                        break;
                }
            }
        }

        protected async Task<TResp> PostAsync<TResp, TReq>(TReq req, string url)
        {
            var content = GetContent(req);

            var response = await _httpClient.PostAsync(url, content);
            TreatErrorsResponse(response);
            return await DeserializeObjectResponse<TResp>(response);
        }

        protected async Task<TResp> GetAsync<TResp, TReq>(TReq req, string url)
        {

            HttpResponseMessage response  = new HttpResponseMessage();

            if (req != null)
             response = await _httpClient.GetAsync($"{url}?{req.GetQueryString(false)}");
            else
                response = await _httpClient.GetAsync($"{url}");

            TreatErrorsResponse(response);
            return await DeserializeObjectResponse<TResp>(response);
        }

        protected async Task<TResp> DeleteAsync<TResp, TReq>(TReq req, string url)
        {

            HttpResponseMessage response = new HttpResponseMessage();

            if (req != null)
                response = await _httpClient.DeleteAsync($"{url}?{req.GetQueryString(false)}");
            else
                response = await _httpClient.DeleteAsync($"{url}");

            TreatErrorsResponse(response);
            return await DeserializeObjectResponse<TResp>(response);
        }

        protected async Task<TResp> PutAsync<TResp, TReq>(TReq req, string url)
        {
            var content = GetContent(req);
            var response = await _httpClient.PutAsync(url, content);
            TreatErrorsResponse(response);
            return await DeserializeObjectResponse<TResp>(response);
        }


        protected StringContent GetContent(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TreatErrorsResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

    }
}
