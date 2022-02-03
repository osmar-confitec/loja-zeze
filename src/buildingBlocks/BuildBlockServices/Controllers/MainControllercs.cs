using BuildBlockCore.DomainObjects;
using BuildBlockCore.Models;
using BuildBlockCore.Utils;
using BuildBlockServices.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildBlockServices.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {

        protected readonly LNotifications _notifications;


        public MainController(LNotifications notifications)
        {
            _notifications = notifications;

        }
        [NonAction]
        public bool IsValid()
        {
            return !_notifications.Any();
        }
        [NonAction]
        protected void ClearErrors()
        {
            _notifications.Clear();
        }

        [NonAction]
        protected IActionResult ReturnModelState(ModelStateDictionary modelState)
        {

            NotifyModelStateErrors();

            return Response(null);

        }
        [NonAction]
        protected async Task<IActionResult> ExecControllerAsync<T>
                            (Func<Task<T>> func)
        {
            try
            {
                return Response(await func());
            }
            catch (Exception ex)
            {

                AddError(ex);
                return Response(null);
            }
        }

        [NonAction]
        protected async Task<IActionResult> ExecControllerAsync(Func<Task> func)
        {
            try
            {
                await func.Invoke();
                return Response(null);
            }
            catch (Exception ex)
            {

                AddError(ex);
                return Response(null);
            }
        }

        //PagedDataResponse


        [NonAction]
        protected async Task<IActionResult> ExecControllerBaseResponseApiPagedAsync<T, Z, Y>(Func<Task<T>> func) where T : class where Z : PagedDataResponse<Y> where Y: class
        {
            try
            {
                var resp1 = (await func.Invoke());
                var resp2 = resp1 as BaseResponseApi<Z>;
                return ResponseBaseResponseApi(resp2);
            }
            catch (Exception ex)
            {
                AddError(ex);
                return ResponseBaseResponseApi<Z>(null);
            }
        }

        [NonAction]
        protected async Task<IActionResult> ExecControllerBaseResponseApiAsync<T,Z>(Func<Task<T>> func) where T: class where Z :class
        {
            try
            {
                var resp1 = (await func.Invoke()) ;
                var resp2 = resp1 as BaseResponseApi<Z>;
                return ResponseBaseResponseApi(resp2);
            }
            catch (Exception ex)
            {
                AddError(ex);
                return ResponseBaseResponseApi<Z>(null);
            }
        }

        //
        [NonAction]
        protected IActionResult ExecController(object result = null)
        {
            try
            {
                return Response(result);
            }
            catch (Exception ex)
            {

                AddError(ex);
                return Response(null);
            }
        }

       [NonAction]
        protected  IActionResult ResponseBaseResponseApi<T>(BaseResponseApi<T> responseApi) where T: class
        {
            if (IsValid())
            {
                return Ok(new
                {
                    success = true,
                    data = responseApi == null ? null : responseApi.Data
                });
            }
            if (_notifications.Any(x => x.TypeNotificationNoty == TypeNotificationNoty.BreakSystem))
            {
                return new ContentResult()
                {
                    StatusCode = 500,
                    ContentType = "application/json",
                    Content = JsonSerializer.Serialize(new
                    {
                        success = false,
                        data = responseApi == null ? null : responseApi.Data,
                        errors = _notifications
                    })
                };
            }
            return BadRequest(new
            {
                success = false,
                data = responseApi == null ? null : responseApi.Data,
                errors = _notifications
            });

        }

        [NonAction]
        protected new IActionResult Response(object result = null)
        {
            if (IsValid())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            if (_notifications.Any(x => x.TypeNotificationNoty == TypeNotificationNoty.BreakSystem))
            {
                return new ContentResult()
                {
                    StatusCode = 500,
                    ContentType = "application/json",
                    Content = JsonSerializer.Serialize(new
                    {
                        success = false,
                        data = result,
                        errors = _notifications
                    })
                };
            }
            return BadRequest(new
            {
                success = false,
                data = result,
                errors = _notifications
            });
        }

        [NonAction]
        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                AddError(new LNotification { Message = erroMsg });

            }
        }
        [NonAction]
        protected void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
                AddError(new LNotification { Message = error.Description });
        }

        [NonAction]
        protected void AddError(Exception except)
        {
             if (except is DomainException)
            {
                _notifications.Add(new LNotification { Message = except.Message });
                return;
            }
            _notifications.Add(new LNotification { TypeNotificationNoty = TypeNotificationNoty.BreakSystem, Message = except.Message });
        }

        [NonAction]
        protected void AddError(LNotification erro)
        {
            _notifications.Add(erro);
        }

    }
}
