using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZeZe.Controllers
{
    public class OrderController : MainController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
