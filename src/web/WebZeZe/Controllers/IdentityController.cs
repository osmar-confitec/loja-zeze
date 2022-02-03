using BuildBlockCore.Identity;
using BuildBlockServices.Request.Users;
using BuildBlockServices.Response.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebZeZe.Services;

namespace WebZeZe.Controllers
{

    [Authorize]
   // [Route("[controller]/[action]")]
    public class IdentityController : MainController
    {
        readonly IAuthService _authService;
        public IdentityController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Identity");
        }

        #region " Deletar "
        //
        [HttpGet]
        [Route("Delete/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
        {
           var deleted =   await _authService.DeleteCustomer(new Guid(id));

            if (IsConstainsErrors(deleted)) return View(deleted);

            return await GetListUsers();

        }
        #endregion

        #region "Atualizar"

        [HttpGet]
        [Route("atualizar-usuario/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] string id)
        {

            var customerResult =   await  _authService.GetCustomer(id);


            return View("Update", new UserUpdateRequest { 
                
                Id = customerResult.Data.Id,
                CPF = customerResult.Data.Cpf,
                Name = customerResult.Data.Name,
                District = customerResult.Data.District,
                City = customerResult.Data.City,
                Complement = customerResult.Data.Complement, 
                Email = customerResult.Data.Email, 
                Number = customerResult.Data.Number, 
                PublicPlace = customerResult.Data.PublicPlace,
                State = customerResult.Data.State, 
                TypeAddress = customerResult.Data.TypeAddress,
                ZipCode = customerResult.Data.ZipCode
               
            });

        }



        [HttpPost]
        [Route("atualizar-usuario/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> UpdateCustomer(UserUpdateRequest userUpdateRequest)
        {
            var responseUpdate = await _authService.UpdateCustomer(userUpdateRequest);

            if (IsConstainsErrors(responseUpdate)) return View(responseUpdate);

            return await GetListUsers();
        }

        #endregion

        #region "Listar"
        [HttpGet]
        [Route("obter-customer")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> GetCustomer()
        {
            return await GetListUsers();

        }

        private async Task<IActionResult> GetListUsers()
        {
            var userListRequest = new UserListRequest();
            var listUser = await _authService.GetCustomer(userListRequest);

            if (IsConstainsErrors(listUser)) return View(userListRequest);

            return View("List", listUser.Data);
        }



        #endregion

        #region "Nova Conta"
        [HttpGet]
        [Route("nova-conta")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> Register(UserRegisterRequest userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);

            var resposta = await _authService.Register(userRegister);

            if (IsConstainsErrors(resposta)) return View(userRegister);

            return await GetListUsers();
        }
        #endregion

        #region  " Login "
        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromQuery]string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginRequest usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _authService.Login(usuarioLogin);

            if (IsConstainsErrors(resposta)) return View(usuarioLogin);

            await SystemLogin(resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }
        #endregion



        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "Identity");
        }




        async Task SystemLogin(UserLoginResponse resposta)
        {
            var token = ObterTokenFormatado(resposta.Data.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.Data.AccessToken));
            claims.AddRange(token.Claims);

            foreach (var item in resposta.Data.UserToken.Claims)
            {
                if (!claims.Any(x => x.Type == item.Type))
                    claims.Add(new Claim(item.Type, item.Value));
            }
            

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

    }
}
