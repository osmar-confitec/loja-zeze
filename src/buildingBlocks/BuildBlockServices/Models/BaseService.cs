using BuildBlockCore.Utils;
using BuildBlockServices.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildBlockServices.Models
{
   public abstract class BaseService
    {

       protected  readonly LNotifications _notification;

        protected BaseService(LNotifications notification)
        {
            _notification = notification ?? new LNotifications();

        }

  
        protected StringContent GetContentJsonUTF8(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializeObjResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cont = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(cont, options);
        }

        protected async Task TreatErrorsResponse<T>(HttpResponseMessage response) 
        {
            if (!response.IsSuccessStatusCode)
            {
               var responseApi =   await DeserializeObjResponse<BaseResponseApi<T>>(response);
                _notification.AddRange(responseApi.Errors);
            }
        }

        protected async Task TreatErrorsResponsePaged<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseApi = await DeserializeObjResponse<BaseResponseApi<T>>(response);
                _notification.AddRange(responseApi.Errors);
            }
        }
    }
}
