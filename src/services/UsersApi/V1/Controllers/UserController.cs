using AutoMapper;
using BuildBlockCore.Identity;
using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Mediator.Messages.Integration;
using BuildBlockCore.Utils;
using BuildBlockMessageBus.MessageBus;
using BuildBlockServices.Controllers;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Request.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [Authorize]
    public class UserController : MainController
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        readonly IMapper _mapper;
        private readonly IMessageBus _bus;


        public UserController(SignInManager<IdentityUser> signInManager,
                                  UserManager<IdentityUser> userManager,
                                  IOptions<AppSettings> appSettings,
                                  IMapper mapper,
                                  IMessageBus bus,
                                  IPasswordHasher<IdentityUser> passwordHasher,
                                  LNotifications notifications)
            : base(notifications)
        {

            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
            _passwordHasher = passwordHasher;
        }

        [HttpPut("atualizar-conta")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> UpdateAccount([FromBody] UserUpdateRequest userUpdate)
        {
            if (!ModelState.IsValid) return ReturnModelState(ModelState);


            var user = await _userManager.FindByIdAsync(userUpdate.Id.ToString());

            if (user == null)
            {
                ModelState.AddModelError("", "Usuário não Encontado");
                NotifyModelStateErrors();
                return Response(null);
            }

            user.Email = userUpdate.Email;
           // user.PasswordHash = _passwordHasher.HashPassword(user, userUpdate.Password);
            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                Response(_mapper.Map<UserUpdateDto>(user));
            else
            { AddError(result); }

            return Response(null);
        }


        [HttpDelete("deletar-conta")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> DeleteAccount([FromQuery] Guid id)
        {

            var identityUser = await _userManager.FindByIdAsync(id.ToString());
            if (identityUser != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(identityUser);
                if (result.Succeeded)
                { return Response(new UserDeleteDto()); }
                else
                { AddError(result); }

                return Response(null);

            }
            else
            {
                ModelState.AddModelError("", "Usuário não Encontado");
                NotifyModelStateErrors();
            }
            return Response(null);
        }

        


        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromQuery] UserLoginRequest userLogin)
        {
            if (!ModelState.IsValid) return ReturnModelState(ModelState);

            return await ExecControllerAsync( async   () => {

        
                var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password,
               false, true);

                if (result.Succeeded)
                {
                    return await GenerateJwt(userLogin.Email);
                }

                if (result.IsLockedOut)
                {
                    AddError(new LNotification { Message = "Usuário temporariamente bloqueado por tentativas inválidas" });
                    return null;
                }

                AddError(new LNotification { Message = "Usuário ou Senha incorretos" });
                return null;
            });
        }


        [HttpPost("nova-conta-login")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterLogin([FromBody] UserRegisterRequest userRegister)
        {

            if (!ModelState.IsValid) return ReturnModelState(ModelState);

           return await ExecControllerAsync( async () => {

                var user = new IdentityUser
                {
                    UserName = userRegister.Email,
                    Email = userRegister.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, userRegister.Password);

                if (result.Succeeded)
                {

                   var respMessage = await _bus.RequestAsync<UserInsertedIntegrationEvent, ResponseMessage>(new 
                       UserInsertedIntegrationEvent(
                                 cPF: userRegister.CPF,
                                 name: userRegister.Name,
                                 id: new Guid(user.Id),
                                 email: user.Email,
                                 number: userRegister.Number,
                                 publicPlace: userRegister.PublicPlace,
                                 zipCode: userRegister.ZipCode,
                                 typeAddress: (int)userRegister.TypeAddress,
                                 city: userRegister.City,
                                 district: userRegister.District,
                                 state: userRegister.State,
                                 complement: userRegister.Complement
                 ));

                   if (respMessage.Notifications.Any())
                   {
                       await _userManager.DeleteAsync(user);
                   }

                   return await GenerateJwt(userRegister.Email);
                }
                foreach (var error in result.Errors)
                {
                    AddError(new LNotification { Message = error.Description });
                }
                return null;
            });

          
        }


        [HttpPost("nova-conta")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegister)
        {

            if (!ModelState.IsValid) return ReturnModelState(ModelState);

            return await ExecControllerAsync(async () => {
                var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);


            if (result.Succeeded)
            {
               var response =  await _bus.RequestAsync<UserInsertedIntegrationEvent, ResponseMessage>(new UserInsertedIntegrationEvent(
                   cPF: userRegister.CPF,
                   name: userRegister.Name,
                   id: new Guid(user.Id),
                   email: user.Email,
                   number: userRegister.Number,
                   publicPlace: userRegister.PublicPlace,
                   zipCode: userRegister.ZipCode,
                   typeAddress: (int)userRegister.TypeAddress,
                   city: userRegister.City,
                   district: userRegister.District,
                   state: userRegister.State,
                   complement: userRegister.Complement
                   ));
                if (response.Notifications.Any())
                {
                    await _userManager.DeleteAsync(user);
                    _notifications.AddRange(response.Notifications);
                    return Response(null);
                }
                return Response(_mapper.Map<UserRegisterDto>(user));
            }

            foreach (var error in result.Errors)
            {
                AddError(new LNotification { Message = error.Description });
            }

           
            return Response(null);
            });
        }


        private async Task<UserLoginDto> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var listClains = await GetClaimsUser(claims, user);
            var encodedToken = Security.GenerateJwt(roles: new List<string>(), addclaims: listClains.ToList(), user.Id, user.Email);
            return GetResponseToken(encodedToken, user, claims);
        }


        private async Task<IEnumerable<Claim>> GetClaimsUser(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            return claims;
        }


        private UserLoginDto GetResponseToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UserLoginDto
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationInHours).TotalSeconds,
                UserToken = new UserTokenDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaimDto { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


    }
}
