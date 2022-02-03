using BuildBlockServices.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZeZe.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool IsConstainsErrors<T>(BaseResponseApi<T> baseResponseApi)
        {
            if (baseResponseApi.Success == false)
            {
                foreach (var item in baseResponseApi.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Message);
                }
            }
            return !baseResponseApi.Success;
        }
    }
}
